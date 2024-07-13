import React from 'react';

import Grid from '@mui/material/Grid/Grid';

import { ApplicationContext } from '../ApplicationProvider';
import Typography from '@mui/material/Typography/Typography';
import { Box, styled } from '@mui/material';
import MuiAccordion, { AccordionProps } from '@mui/material/Accordion';
import MuiAccordionSummary, { AccordionSummaryProps } from '@mui/material/AccordionSummary';
import MuiAccordionDetails from '@mui/material/AccordionDetails';
import { IMonitoredItem, browseChildren } from '../opcua-utils';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ArrowForwardIosSharpIcon from '@mui/icons-material/ArrowForwardIosSharp';
import { Paper } from '@mui/material';
import * as Battery from '../opcua/batterypassport-constants.ts';
import * as OpcUa from '../opcua'; 
import * as Web from '../Web'; 
export interface ValueDisplayWidgetProps {
   label: string
   value: React.ReactNode,
   units?: string
}

const ValueDisplayWidget = ({ label, value, units }: ValueDisplayWidgetProps) => {
   return (
      <Box p={6}>
         <Typography variant='body1' fontSize={'smaller'} fontWeight={'bolder'} color={'grey'}>{label}</Typography>
         <Box display={'flex'} alignItems={'flex-end'}>
            <Typography variant='body1'>{value}</Typography>
            <Typography variant='body1' ml={4}>{units}</Typography>
         </Box>
      </Box>
   );
}

function formatValue(fieldName: string, monitoredItems?: Map<number, IMonitoredItem>): React.ReactNode {
   if (monitoredItems) {
      const dv = Array.from(monitoredItems.values()).find(x => x.name === fieldName)?.value;
      if (dv) {
         if (OpcUa.StatusCode.isBad(dv.StatusCode)) {
            return <React.Fragment>{OpcUa.StatusCodes[dv.StatusCode ?? 0]}</React.Fragment >;
         }
         if (dv.Value?.Type === 21) { // LocalizedText
            return <React.Fragment>{dv.Value?.Body?.Text};</React.Fragment >;
         }
         if (dv.Value?.Type === 12) { // String
            if (dv.Value?.Body.startsWith("http")) {
               return <React.Fragment><a className='m-0 p-0 nav-link' target='_blank' rel='noopener noreferrer' href={dv.Value?.Body}>{dv.Value?.Body}</a></React.Fragment>
            }
            return <React.Fragment>{dv.Value?.Body}</React.Fragment>;
         }
         if (dv.Value?.Type === 11) { // Double
            return <React.Fragment>{Number(dv.Value?.Body).toFixed(2)}</React.Fragment>;
         }
         if ((dv.Value?.Type ?? 0) > 2 && (dv.Value?.Type ?? 0) < 10) { // Integers
            return <React.Fragment>{Number(dv.Value?.Body)}</React.Fragment>;
         }
         if (dv.Value?.Type === 13) { // DateTime
            return <React.Fragment>{Web.formatDate(dv.Value?.Body)}</React.Fragment>;
         }
         return <React.Fragment>{JSON.stringify(dv.Value?.Body)}</React.Fragment>;
      }
   }
   return <React.Fragment>{"---"}</React.Fragment>;
}

export interface CurrentStateWidgetProps {
   batteryId?: string
}

export const CurrentStateWidget = ({ batteryId }: CurrentStateWidgetProps) => {
   const inProgress = React.useRef(false);
   const context = React.useContext(ApplicationContext);
   const monitoredItems = context.monitoredItems;
   const createMonitoredItems = context.createMonitoredItems;

   const uri = 'nsu=http://opcfoundation.org/UA/BatteryPassport/';

   React.useEffect(() => {
      const controller = new AbortController();
      if (monitoredItems?.size) {
         if (Array.from(monitoredItems?.values()).find((item) => item.nodeId === batteryId && item.serverHandle)) {
            inProgress.current = false;
            return;
         }
      }
      if (inProgress.current || !batteryId) {
         return;
      }
      inProgress.current = true;
      doCreateMonitoredItems();
      async function doCreateMonitoredItems() {
         createMonitoredItems([
            {
               path: ["Battery", "ChargeRemaining"],
               name: "ChargeRemaining",
               nodeId: batteryId,
               browsePath: [`${uri};CurrentState`, `${uri};ChargeRemaining`]
            },
            {
               path: ["Battery", "TimeRemaining"],
               name: "TimeRemaining",
               nodeId: batteryId,
               browsePath: [`${uri};CurrentState`, `${uri};TimeRemaining`]
            },
            {
               path: ["Battery", "PowerIn"],
               name: "PowerIn",
               nodeId: batteryId,
               browsePath: [`${uri};CurrentState`, `${uri};PowerIn`]
            },
            {
               path: ["Battery", "PowerOut"],
               name: "PowerOut",
               nodeId: batteryId,
               browsePath: [`${uri};CurrentState`, `${uri};PowerOut`]
            },
            {
               path: ["Battery", "Voltage"],
               name: "Voltage",
               nodeId: batteryId,
               browsePath: [`${uri};CurrentState`, `${uri};Voltage`]
            },
            {
               path: ["Battery", "Temperature"],
               name: "Temperature",
               nodeId: batteryId,
               browsePath: [`${uri};CurrentState`, `${uri};Temperature`]
            }] as IMonitoredItem[],
            controller)
         .catch(() => inProgress.current = false);
      }
      return () => {
         // controller.abort();
      };
   }, [batteryId, monitoredItems, createMonitoredItems]);

   return (
      <Grid container spacing={2}>
         <Grid item xs={4}>
            <ValueDisplayWidget label={'Charge Remaining'} value={formatValue("ChargeRemaining", monitoredItems)} units={'%'} />
         </Grid>
         <Grid item xs={4}>
            <ValueDisplayWidget label={'Time Remaing'} value={formatValue("TimeRemaining", monitoredItems)} units={'Minutes'} />
         </Grid>
         <Grid item xs={4}>
            <ValueDisplayWidget label={'Power In'} value={formatValue("PowerIn", monitoredItems)} units={'kW'} />
         </Grid>
         <Grid item xs={4}>
            <ValueDisplayWidget label={'Power Out'} value={formatValue("PowerOut", monitoredItems)} units={'kW'} />
         </Grid>
         <Grid item xs={4}>
            <ValueDisplayWidget label={'Voltage'} value={formatValue("Voltage", monitoredItems)} units={'V'} />
         </Grid>
         <Grid item xs={4}>
            <ValueDisplayWidget label={'Temperature'} value={formatValue("Temperature", monitoredItems)} units={'ºC'} />
         </Grid>
      </Grid>
   );
}

function insertSpaces(input: string): string {
   let value = input.replace(/[A-Z]/g, (match) => ` ${match}`);
   value = value.replace(/_/g, () => ` `);
   return value;
}

export interface PassportPropertiesPanelProps {
   batteryId?: string,
   category: string,
   properties: string[],
}

export const PassportPropertiesPanel = ({ batteryId, category, properties }: PassportPropertiesPanelProps) => {
   const inProgress = React.useRef(false);
   const context = React.useContext(ApplicationContext);
   const monitoredItems = context.monitoredItems;
   const createMonitoredItems = context.createMonitoredItems;

   const uri = 'nsu=http://opcfoundation.org/UA/BatteryPassport/';

   React.useEffect(() => {
      const controller = new AbortController();
      if (monitoredItems?.size) {
         if (Array.from(monitoredItems?.values()).find((item) => item.path[1] === category && item.serverHandle)) {
            inProgress.current = false;
            return;
         }
      }
      if (inProgress.current || !batteryId) {
         return;
      }
      inProgress.current = true;
      doCreateMonitoredItems();
      async function doCreateMonitoredItems() {
         const items = properties.map((property) => {
            return {
               path: ["Battery", category, property],
               name: property,
               nodeId: batteryId,
               browsePath: [`${uri};Passport`, `${uri};${category}`, `${uri};${property}`]
            } as IMonitoredItem
         });
         createMonitoredItems(items, controller)
         .catch(() => inProgress.current = false);
      }
      return () => {
         // controller.abort();
      };
   }, [batteryId, category, properties, monitoredItems, createMonitoredItems]);

   return (
      <Grid container spacing={2}>
         {properties.map((property) => {
            return (
               <Grid key={property} item xs={12}>
                  <ValueDisplayWidget label={insertSpaces(property)} value={formatValue(property, monitoredItems)} />
               </Grid>
            );
         })}
      </Grid>
   );
}

const Accordion = styled((props: AccordionProps) => (
   <MuiAccordion disableGutters elevation={0} square {...props} />
))(({ theme }) => ({
   border: `1px solid ${theme.palette.divider}`,
   '&:not(:last-child)': {
      borderBottom: 0,
   },
   '&::before': {
      display: 'none',
   },
}));

const AccordionSummary = styled((props: AccordionSummaryProps) => (
   <MuiAccordionSummary
      expandIcon={<ArrowForwardIosSharpIcon sx={{ fontSize: '0.9rem' }} />}
      {...props}
   />
))(({ theme }) => ({
   backgroundColor:
      theme.palette.mode === 'dark'
         ? 'rgba(255, 255, 255, .05)'
         : 'rgb(226, 234, 244)',
   flexDirection: 'row-reverse',
   '& .MuiAccordionSummary-expandIconWrapper.Mui-expanded': {
      transform: 'rotate(90deg)',
   },
   '& .MuiAccordionSummary-content': {
      marginLeft: theme.spacing(1),
   },
}));

const AccordionDetails = styled(MuiAccordionDetails)(({ theme }) => ({
   padding: theme.spacing(2),
   borderTop: '1px solid rgba(0, 0, 0, .125)',
}));

export const BatteryPassportPage = () => {
   const [selection, setSelection] = React.useState<OpcUa.ReferenceDescription | undefined>();
   const [inProgress, setInProgress] = React.useState<boolean>(false);
   const context = React.useContext(ApplicationContext);
   const user = context?.userContext?.user;
   const requestTimeout = context.requestTimeout;

   React.useEffect(() => {
      const controller = new AbortController();
      if (!selection && !inProgress) {
         setInProgress(true);
         browseChildren(Battery.ObjectIds.Batteries, requestTimeout, controller, user).then((result) => {
            setSelection(result?.find((x) => x.reference.TypeDefinition === Battery.ObjectTypeIds.BatteryType)?.reference);
         }).finally(() => setInProgress(false));
      }
      return () => {
         // controller.abort();
      };
   }, [selection, inProgress, user, requestTimeout]);

   return (
      <Paper variant='outlined' sx={{ backgroundColor: '#FBF9EF', borderColor: '#C59E01', m: 2, p: 6 }}>
         <Paper elevation={3} sx={{ p: 10, backgroundColor: 'rgb(226, 234, 244)' }} >
            <Typography variant='h6'>{selection?.DisplayName?.Text ?? "Battery"}</Typography>
         </Paper>
         <CurrentStateWidget batteryId={selection?.NodeId} />
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel1-content"
               id="panel1-header"

            >
               <Typography variant='body1' sx={{ px: 6 }}>General battery and manufacturer information</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'GeneralBatteryAndManufacturerInformation'}
                  properties={[
                     'BatteryUniqueIdentifier',
                     'ManufacturersIdentification',
                     'ManufacturingDate',
                     'ManufacturingPlace',
                     'BatteryCategory',
                     'BatteryWeight',
                     'BatteryStatus'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel2-content"
               id="panel2-header"
            >
               <Typography variant='body1'>Compliance, labels and certifications</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'Compliance_LabelsAndCertifications'}
                  properties={[
                     'ResultsOfTestsReports',
                     'SeparateCollectionSymbol',
                     'MeaningOfLabelsAndSymbols',
                     'CadmiumAndLeadSymbols',
                     'IDOfEUDeclarationOfConformity',
                     'EUDeclarationOfConformity'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel3-content"
               id="panel3-header"
            >
               <Typography variant='body1'>Battery materials and composition</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'BatteryMaterialsAndComposition'}
                  properties={[
                     'CriticalRawMaterials',
                     'BatteryChemistry',
                     'NameOfTheCathode_Anode_ElectrolyteMaterials',
                     'RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials',
                     'WeightOfTheCathode_Anode_ElectrolyteMaterials',
                     'NameOfHazardousSubstances',
                     'RelatedIdentifiersOfHazardousSubstances',
                     'LocationOfHazardousSubstances',
                     'ConcentrationRangeOfHazardousSubstances',
                     'ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel4-content"
               id="panel4-header"
            >
               <Typography variant='body1'>Carbon footprint</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'CarbonFootprint'}
                  properties={[
                     'BatteryCarbonFootprint',
                     'ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing',
                     'ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction',
                     'ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution',
                     'ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling',
                     'CarbonFootprintPerformanceClass',
                     'WebLinkToPublicCarbonFootprintStudy'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel5-content"
               id="panel5-header"
            >
               <Typography variant='body1'>Supply chain due diligence</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'SupplyChainDueDiligence'}
                  properties={[
                     'InformationOfTheDueDiligenceReport',
                     'ThirdPartySupplyChainAssurances',
                     'EUTaxonomyDisclosureStatement',
                     'SustainabilityReport'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel6-content"
               id="panel6-header"
            >
               <Typography variant='body1'>Circularity and resource efficiency</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'CircularityAndResourceEfficiency'}
                  properties={[
                     'ManualForRemovalOfTheBatteryFromTheAppliance',
                     'ManualForDisassemblyAndDismantlingOfTheBatteryPack',
                     'PostalAddressOfSourcesForSpareParts',
                     'E_MailAddressOfSourcesForSpareParts',
                     'WebAddressOfSourcesForSpareParts',
                     'PartNumbersForComponents',
                     'ExtinguishingAgent',
                     'SafetyMeasures_Instructions',
                     'Pre_ConsumerRecycledNickelShare',
                     'Pre_ConsumerRecycledCobaltShare',
                     'Pre_ConsumerRecycledLithiumShare',
                     'Pre_ConsumerRecycledLeadShare',
                     'Post_ConsumerRecycledNickelShare',
                     'Post_ConsumerRecycledCobaltShare',
                     'Post_ConsumerRecycledLithiumShare',
                     'Post_ConsumerRecycledLeadShare',
                     'RenewableContentShare',
                     'RoleOfEnd_UsersInContributingToWastePrevention',
                     'RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries',
                     'InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
         <Accordion>
            <AccordionSummary
               expandIcon={<ExpandMoreIcon />}
               aria-controls="panel7-content"
               id="panel7-header"
            >
               <Typography variant='body1'>Performance and durability</Typography>
            </AccordionSummary>
            <AccordionDetails>
               <PassportPropertiesPanel
                  batteryId={selection?.NodeId}
                  category={'PerformanceAndDurability'}
                  properties={[
                     'TimeSpentChargingDuringExtremeTemperaturesAboveBoundary',
                     'InformationOnAccidents',
                     'NumberOfDeepDischargeEvents',
                     'NumberOfOverchargeEvents',
                     'CertifiedUsableBatteryEnergy_UBEcertified',
                     'RemainingUsableBatteryEnergy_UBEmeasured',
                     'StateOfCertifiedEnergy_SOCE',
                     'InitialSelf_DischargingRate',
                     'CurrentSelf_DischargingRate',
                     'EvolutionOfSelf_DischargingRates',
                     'RatedCapacity',
                     'CapacityFade',
                     'StateOfCharge_SoC',
                     'NominalVoltage',
                     'MinimumVoltage',
                     'MaximumVoltage',
                     'OriginalPowerCapability',
                     'RemainingPowerCapability',
                     'PowerCapabilityFade',
                     'RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh',
                     'InitialRoundTripEnergyEfficiency',
                     'RoundTripEnergyEfficiencyAt50_OfCycleLife',
                     'RemainingRoundTripEnergyEfficiency',
                     'RoundTripEnergyEfficiencyFade',
                     'InitialInternalResistanceOnBatteryCellLevel',
                     'CurrentInternalResistanceOnBatteryCellLevel',
                     'InternalResistanceIncreaseOnBatteryCellLevel',
                     'InitialInternalResistanceOnBatteryPackLevel',
                     'CurrentInternalResistanceOnBatteryPackLevel',
                     'InitialInternalResistanceOnBatteryModuleLevel',
                     'InternalResistanceIncreaseOnBatteryModuleLevel',
                     'ExpectedLifetime_NumberOfCharge_DischargeCycles',
                     'NumberOf_Full_Charge_DischargeCycles',
                     'Cycle_LifeReferenceTest',
                     'C_RateOfRelevantCycle_LifeTest',
                     'EnergyThroughput',
                     'CapacityThroughput',
                     'CapacityThresholdForExhaustion',
                     'SOCEThresholdForExhaustion',
                     'WarrantyPeriodOfTheBattery',
                     'DateOfPuttingTheBatteryIntoService',
                     'TemperatureRangeIdleState_LowerBoundary',
                     'TemperatureRangeIdleState_UpperBoundary',
                     'TimeSpentInExtremeTemperaturesAboveBoundary',
                     'TimeSpentInExtremeTemperaturesBelowBoundary'
                  ]}
               />
            </AccordionDetails>
         </Accordion>
      </Paper>
   );
};

export default BatteryPassportPage;