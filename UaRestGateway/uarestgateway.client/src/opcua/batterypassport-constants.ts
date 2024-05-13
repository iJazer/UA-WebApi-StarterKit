
export const BatteryPassportNamespaceUri = 'http://opcfoundation.org/UA/BatteryPassport/';
export enum DataTypeIds {
   BatteryPassportDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3857',
   BatteryMaterialsAndCompositionDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3858',
   CarbonFootprintDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3859',
   CircularityAndResourceEfficiencyDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3860',
   Compliance_LabelsAndCertificationsDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3861',
   GeneralBatteryAndManufacturerInformationDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3862',
   PerformanceAndDurabilityDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3863',
   SupplyChainDueDiligenceDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3864'
}

export enum MethodIds {
   BatteryType_Package_Open = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5393',
   BatteryType_Package_Close = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5396',
   BatteryType_Package_Read = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5398',
   BatteryType_Package_Write = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5401',
   BatteryType_Package_GetPosition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5403',
   BatteryType_Package_SetPosition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5406'
}

export enum ObjectIds {
   Batteries = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3937',
   Compliance_LabelsAndCertificationsDataType_Encoding_DefaultBinary = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3869',
   Compliance_LabelsAndCertificationsDataType_Encoding_DefaultXml = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3901',
   Compliance_LabelsAndCertificationsDataType_Encoding_DefaultJson = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3933'
}

export enum ObjectTypeIds {
   BatteryCurrentStateType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3938',
   BatteryType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=5384',
   SubmodelType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2',
   BatteryPassportType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2497',
   BatteryMaterialsAndCompositionType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=16',
   CarbonFootprintType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=160',
   CircularityAndResourceEfficiencyType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=252',
   Compliance_LabelsAndCertificationsType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2146',
   GeneralBatteryAndManufacturerInformationType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=592',
   PerformanceAndDurabilityType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=684',
   SupplyChainDueDiligenceType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1322'
}

export enum VariableIds {
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2147',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Id = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2148',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_SubCategory = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2149',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Definition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2150',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Requirements = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2151',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Regulations = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2152',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_AccessRights = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2153',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Behaviour = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2154',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Granularity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2155',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Pack = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2156',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Module = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2157',
   Compliance_LabelsAndCertificationsType_ResultsOfTestsReports_Cell = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2158',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2160',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Id = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2161',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_SubCategory = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2162',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Definition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2163',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Requirements = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2164',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Regulations = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2165',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_AccessRights = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2166',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Behaviour = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2167',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Granularity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2168',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Pack = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2169',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Module = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2170',
   Compliance_LabelsAndCertificationsType_SeparateCollectionSymbol_Cell = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2171',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2173',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Id = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2174',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_SubCategory = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2175',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Definition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2176',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Requirements = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2177',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Regulations = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2178',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_AccessRights = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2179',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Behaviour = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2180',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Granularity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2181',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Pack = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2182',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Module = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2183',
   Compliance_LabelsAndCertificationsType_MeaningOfLabelsAndSymbols_Cell = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2184',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2186',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Id = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2187',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_SubCategory = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2188',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Definition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2189',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Requirements = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2190',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Regulations = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2191',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_AccessRights = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2192',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Behaviour = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2193',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Granularity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2194',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Pack = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2195',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Module = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2196',
   Compliance_LabelsAndCertificationsType_CadmiumAndLeadSymbols_Cell = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2197',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2199',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Id = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2200',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_SubCategory = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2201',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Definition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2202',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Requirements = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2203',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Regulations = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2204',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_AccessRights = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2205',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Behaviour = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2206',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Granularity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2207',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Pack = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2208',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Module = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2209',
   Compliance_LabelsAndCertificationsType_EUDeclarationOfConformity_Cell = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2210',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2212',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Id = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2213',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_SubCategory = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2214',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Definition = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2215',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Requirements = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2216',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Regulations = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2217',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_AccessRights = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2218',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Behaviour = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2219',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Granularity = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2220',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Pack = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2221',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Module = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2222',
   Compliance_LabelsAndCertificationsType_IDOfEUDeclarationOfConformity_Cell = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=2223',
   BatteryPassport_BinarySchema = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1375',
   BatteryPassport_BinarySchema_NamespaceUri = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1377',
   BatteryPassport_BinarySchema_Deprecated = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1378',
   BatteryPassport_BinarySchema_BatteryPassportDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3873',
   BatteryPassport_BinarySchema_BatteryMaterialsAndCompositionDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3876',
   BatteryPassport_BinarySchema_CarbonFootprintDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3879',
   BatteryPassport_BinarySchema_CircularityAndResourceEfficiencyDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3882',
   BatteryPassport_BinarySchema_Compliance_LabelsAndCertificationsDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3885',
   BatteryPassport_BinarySchema_GeneralBatteryAndManufacturerInformationDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3888',
   BatteryPassport_BinarySchema_PerformanceAndDurabilityDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3891',
   BatteryPassport_BinarySchema_SupplyChainDueDiligenceDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3894',
   BatteryPassport_XmlSchema = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1400',
   BatteryPassport_XmlSchema_NamespaceUri = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1402',
   BatteryPassport_XmlSchema_Deprecated = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=1403',
   BatteryPassport_XmlSchema_BatteryPassportDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3905',
   BatteryPassport_XmlSchema_BatteryMaterialsAndCompositionDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3908',
   BatteryPassport_XmlSchema_CarbonFootprintDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3911',
   BatteryPassport_XmlSchema_CircularityAndResourceEfficiencyDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3914',
   BatteryPassport_XmlSchema_Compliance_LabelsAndCertificationsDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3917',
   BatteryPassport_XmlSchema_GeneralBatteryAndManufacturerInformationDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3920',
   BatteryPassport_XmlSchema_PerformanceAndDurabilityDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3923',
   BatteryPassport_XmlSchema_SupplyChainDueDiligenceDataType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3926'
}

export enum VariableTypeIds {
   SubmodelPropertyType = 'nsu=http://opcfoundation.org/UA/BatteryPassport/;i=3'
}

export class StatusCode {
   public static isGood(code?: number): boolean {
      return !code || (code & 0xD0000000) === 0;
   }
   public static isUncertain(code?: number): boolean {
      return (code ?? 0 & 0x40000000) !== 0;
   }
   public static isBad(code?: number): boolean {
      return (code ?? 0 & 0x80000000) !== 0;
   }
}