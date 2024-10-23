/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace BatteryPassport
{
    #region BatteryPassportDataType Class
    #if (!OPCUA_EXCLUDE_BatteryPassportDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class BatteryPassportDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public BatteryPassportDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_batteryMaterialsAndComposition = new BatteryMaterialsAndCompositionDataType();
            m_carbonFootprint = new CarbonFootprintDataType();
            m_circularityAndResourceEfficiency = new CircularityAndResourceEfficiencyDataType();
            m_compliance_LabelsAndCertifications = new Compliance_LabelsAndCertificationsDataType();
            m_generalBatteryAndManufacturerInformation = new GeneralBatteryAndManufacturerInformationDataType();
            m_performanceAndDurability = new PerformanceAndDurabilityDataType();
            m_supplyChainDueDiligence = new SupplyChainDueDiligenceDataType();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "BatteryMaterialsAndComposition", IsRequired = false, Order = 1)]
        public BatteryMaterialsAndCompositionDataType BatteryMaterialsAndComposition
        {
            get
            {
                return m_batteryMaterialsAndComposition;
            }

            set
            {
                m_batteryMaterialsAndComposition = value;

                if (value == null)
                {
                    m_batteryMaterialsAndComposition = new BatteryMaterialsAndCompositionDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "CarbonFootprint", IsRequired = false, Order = 2)]
        public CarbonFootprintDataType CarbonFootprint
        {
            get
            {
                return m_carbonFootprint;
            }

            set
            {
                m_carbonFootprint = value;

                if (value == null)
                {
                    m_carbonFootprint = new CarbonFootprintDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "CircularityAndResourceEfficiency", IsRequired = false, Order = 3)]
        public CircularityAndResourceEfficiencyDataType CircularityAndResourceEfficiency
        {
            get
            {
                return m_circularityAndResourceEfficiency;
            }

            set
            {
                m_circularityAndResourceEfficiency = value;

                if (value == null)
                {
                    m_circularityAndResourceEfficiency = new CircularityAndResourceEfficiencyDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "Compliance_LabelsAndCertifications", IsRequired = false, Order = 4)]
        public Compliance_LabelsAndCertificationsDataType Compliance_LabelsAndCertifications
        {
            get
            {
                return m_compliance_LabelsAndCertifications;
            }

            set
            {
                m_compliance_LabelsAndCertifications = value;

                if (value == null)
                {
                    m_compliance_LabelsAndCertifications = new Compliance_LabelsAndCertificationsDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "GeneralBatteryAndManufacturerInformation", IsRequired = false, Order = 5)]
        public GeneralBatteryAndManufacturerInformationDataType GeneralBatteryAndManufacturerInformation
        {
            get
            {
                return m_generalBatteryAndManufacturerInformation;
            }

            set
            {
                m_generalBatteryAndManufacturerInformation = value;

                if (value == null)
                {
                    m_generalBatteryAndManufacturerInformation = new GeneralBatteryAndManufacturerInformationDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "PerformanceAndDurability", IsRequired = false, Order = 6)]
        public PerformanceAndDurabilityDataType PerformanceAndDurability
        {
            get
            {
                return m_performanceAndDurability;
            }

            set
            {
                m_performanceAndDurability = value;

                if (value == null)
                {
                    m_performanceAndDurability = new PerformanceAndDurabilityDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "SupplyChainDueDiligence", IsRequired = false, Order = 7)]
        public SupplyChainDueDiligenceDataType SupplyChainDueDiligence
        {
            get
            {
                return m_supplyChainDueDiligence;
            }

            set
            {
                m_supplyChainDueDiligence = value;

                if (value == null)
                {
                    m_supplyChainDueDiligence = new SupplyChainDueDiligenceDataType();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.BatteryPassportDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.BatteryPassportDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.BatteryPassportDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.BatteryPassportDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteEncodeable("BatteryMaterialsAndComposition", BatteryMaterialsAndComposition, typeof(BatteryMaterialsAndCompositionDataType));
            encoder.WriteEncodeable("CarbonFootprint", CarbonFootprint, typeof(CarbonFootprintDataType));
            encoder.WriteEncodeable("CircularityAndResourceEfficiency", CircularityAndResourceEfficiency, typeof(CircularityAndResourceEfficiencyDataType));
            encoder.WriteEncodeable("Compliance_LabelsAndCertifications", Compliance_LabelsAndCertifications, typeof(Compliance_LabelsAndCertificationsDataType));
            encoder.WriteEncodeable("GeneralBatteryAndManufacturerInformation", GeneralBatteryAndManufacturerInformation, typeof(GeneralBatteryAndManufacturerInformationDataType));
            encoder.WriteEncodeable("PerformanceAndDurability", PerformanceAndDurability, typeof(PerformanceAndDurabilityDataType));
            encoder.WriteEncodeable("SupplyChainDueDiligence", SupplyChainDueDiligence, typeof(SupplyChainDueDiligenceDataType));

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            BatteryMaterialsAndComposition = (BatteryMaterialsAndCompositionDataType)decoder.ReadEncodeable("BatteryMaterialsAndComposition", typeof(BatteryMaterialsAndCompositionDataType));
            CarbonFootprint = (CarbonFootprintDataType)decoder.ReadEncodeable("CarbonFootprint", typeof(CarbonFootprintDataType));
            CircularityAndResourceEfficiency = (CircularityAndResourceEfficiencyDataType)decoder.ReadEncodeable("CircularityAndResourceEfficiency", typeof(CircularityAndResourceEfficiencyDataType));
            Compliance_LabelsAndCertifications = (Compliance_LabelsAndCertificationsDataType)decoder.ReadEncodeable("Compliance_LabelsAndCertifications", typeof(Compliance_LabelsAndCertificationsDataType));
            GeneralBatteryAndManufacturerInformation = (GeneralBatteryAndManufacturerInformationDataType)decoder.ReadEncodeable("GeneralBatteryAndManufacturerInformation", typeof(GeneralBatteryAndManufacturerInformationDataType));
            PerformanceAndDurability = (PerformanceAndDurabilityDataType)decoder.ReadEncodeable("PerformanceAndDurability", typeof(PerformanceAndDurabilityDataType));
            SupplyChainDueDiligence = (SupplyChainDueDiligenceDataType)decoder.ReadEncodeable("SupplyChainDueDiligence", typeof(SupplyChainDueDiligenceDataType));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            BatteryPassportDataType value = encodeable as BatteryPassportDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_batteryMaterialsAndComposition, value.m_batteryMaterialsAndComposition)) return false;
            if (!Utils.IsEqual(m_carbonFootprint, value.m_carbonFootprint)) return false;
            if (!Utils.IsEqual(m_circularityAndResourceEfficiency, value.m_circularityAndResourceEfficiency)) return false;
            if (!Utils.IsEqual(m_compliance_LabelsAndCertifications, value.m_compliance_LabelsAndCertifications)) return false;
            if (!Utils.IsEqual(m_generalBatteryAndManufacturerInformation, value.m_generalBatteryAndManufacturerInformation)) return false;
            if (!Utils.IsEqual(m_performanceAndDurability, value.m_performanceAndDurability)) return false;
            if (!Utils.IsEqual(m_supplyChainDueDiligence, value.m_supplyChainDueDiligence)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (BatteryPassportDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            BatteryPassportDataType clone = (BatteryPassportDataType)base.MemberwiseClone();

            clone.m_batteryMaterialsAndComposition = (BatteryMaterialsAndCompositionDataType)Utils.Clone(this.m_batteryMaterialsAndComposition);
            clone.m_carbonFootprint = (CarbonFootprintDataType)Utils.Clone(this.m_carbonFootprint);
            clone.m_circularityAndResourceEfficiency = (CircularityAndResourceEfficiencyDataType)Utils.Clone(this.m_circularityAndResourceEfficiency);
            clone.m_compliance_LabelsAndCertifications = (Compliance_LabelsAndCertificationsDataType)Utils.Clone(this.m_compliance_LabelsAndCertifications);
            clone.m_generalBatteryAndManufacturerInformation = (GeneralBatteryAndManufacturerInformationDataType)Utils.Clone(this.m_generalBatteryAndManufacturerInformation);
            clone.m_performanceAndDurability = (PerformanceAndDurabilityDataType)Utils.Clone(this.m_performanceAndDurability);
            clone.m_supplyChainDueDiligence = (SupplyChainDueDiligenceDataType)Utils.Clone(this.m_supplyChainDueDiligence);

            return clone;
        }
        #endregion

        #region Private Fields
        private BatteryMaterialsAndCompositionDataType m_batteryMaterialsAndComposition;
        private CarbonFootprintDataType m_carbonFootprint;
        private CircularityAndResourceEfficiencyDataType m_circularityAndResourceEfficiency;
        private Compliance_LabelsAndCertificationsDataType m_compliance_LabelsAndCertifications;
        private GeneralBatteryAndManufacturerInformationDataType m_generalBatteryAndManufacturerInformation;
        private PerformanceAndDurabilityDataType m_performanceAndDurability;
        private SupplyChainDueDiligenceDataType m_supplyChainDueDiligence;
        #endregion
    }

    #region BatteryPassportDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfBatteryPassportDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "BatteryPassportDataType")]
    public partial class BatteryPassportDataTypeCollection : List<BatteryPassportDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public BatteryPassportDataTypeCollection() {}

        /// <remarks />
        public BatteryPassportDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public BatteryPassportDataTypeCollection(IEnumerable<BatteryPassportDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator BatteryPassportDataTypeCollection(BatteryPassportDataType[] values)
        {
            if (values != null)
            {
                return new BatteryPassportDataTypeCollection(values);
            }

            return new BatteryPassportDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator BatteryPassportDataType[](BatteryPassportDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (BatteryPassportDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            BatteryPassportDataTypeCollection clone = new BatteryPassportDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((BatteryPassportDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region BatteryMaterialsAndCompositionDataType Class
    #if (!OPCUA_EXCLUDE_BatteryMaterialsAndCompositionDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class BatteryMaterialsAndCompositionDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public BatteryMaterialsAndCompositionDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_criticalRawMaterials = null;
            m_batteryChemistry = null;
            m_nameOfTheCathode_Anode_ElectrolyteMaterials = null;
            m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials = null;
            m_weightOfTheCathode_Anode_ElectrolyteMaterials = (double)0;
            m_nameOfHazardousSubstances = null;
            m_hazardClassesAnd_OrCategoriesOfHazardousSubstances = null;
            m_relatedIdentifiersOfHazardousSubstances = null;
            m_locationOfHazardousSubstances = null;
            m_concentrationRangeOfHazardousSubstances = (double)0;
            m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "CriticalRawMaterials", IsRequired = false, Order = 1)]
        public string CriticalRawMaterials
        {
            get { return m_criticalRawMaterials;  }
            set { m_criticalRawMaterials = value; }
        }

        /// <remarks />
        [DataMember(Name = "BatteryChemistry", IsRequired = false, Order = 2)]
        public string BatteryChemistry
        {
            get { return m_batteryChemistry;  }
            set { m_batteryChemistry = value; }
        }

        /// <remarks />
        [DataMember(Name = "NameOfTheCathode_Anode_ElectrolyteMaterials", IsRequired = false, Order = 3)]
        public string NameOfTheCathode_Anode_ElectrolyteMaterials
        {
            get { return m_nameOfTheCathode_Anode_ElectrolyteMaterials;  }
            set { m_nameOfTheCathode_Anode_ElectrolyteMaterials = value; }
        }

        /// <remarks />
        [DataMember(Name = "RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials", IsRequired = false, Order = 4)]
        public string RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials
        {
            get { return m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials;  }
            set { m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials = value; }
        }

        /// <remarks />
        [DataMember(Name = "WeightOfTheCathode_Anode_ElectrolyteMaterials", IsRequired = false, Order = 5)]
        public double WeightOfTheCathode_Anode_ElectrolyteMaterials
        {
            get { return m_weightOfTheCathode_Anode_ElectrolyteMaterials;  }
            set { m_weightOfTheCathode_Anode_ElectrolyteMaterials = value; }
        }

        /// <remarks />
        [DataMember(Name = "NameOfHazardousSubstances", IsRequired = false, Order = 6)]
        public string NameOfHazardousSubstances
        {
            get { return m_nameOfHazardousSubstances;  }
            set { m_nameOfHazardousSubstances = value; }
        }

        /// <remarks />
        [DataMember(Name = "HazardClassesAnd_OrCategoriesOfHazardousSubstances", IsRequired = false, Order = 7)]
        public string HazardClassesAnd_OrCategoriesOfHazardousSubstances
        {
            get { return m_hazardClassesAnd_OrCategoriesOfHazardousSubstances;  }
            set { m_hazardClassesAnd_OrCategoriesOfHazardousSubstances = value; }
        }

        /// <remarks />
        [DataMember(Name = "RelatedIdentifiersOfHazardousSubstances", IsRequired = false, Order = 8)]
        public string RelatedIdentifiersOfHazardousSubstances
        {
            get { return m_relatedIdentifiersOfHazardousSubstances;  }
            set { m_relatedIdentifiersOfHazardousSubstances = value; }
        }

        /// <remarks />
        [DataMember(Name = "LocationOfHazardousSubstances", IsRequired = false, Order = 9)]
        public string LocationOfHazardousSubstances
        {
            get { return m_locationOfHazardousSubstances;  }
            set { m_locationOfHazardousSubstances = value; }
        }

        /// <remarks />
        [DataMember(Name = "ConcentrationRangeOfHazardousSubstances", IsRequired = false, Order = 10)]
        public double ConcentrationRangeOfHazardousSubstances
        {
            get { return m_concentrationRangeOfHazardousSubstances;  }
            set { m_concentrationRangeOfHazardousSubstances = value; }
        }

        /// <remarks />
        [DataMember(Name = "ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety", IsRequired = false, Order = 11)]
        public string ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety
        {
            get { return m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety;  }
            set { m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.BatteryMaterialsAndCompositionDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.BatteryMaterialsAndCompositionDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.BatteryMaterialsAndCompositionDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.BatteryMaterialsAndCompositionDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteString("CriticalRawMaterials", CriticalRawMaterials);
            encoder.WriteString("BatteryChemistry", BatteryChemistry);
            encoder.WriteString("NameOfTheCathode_Anode_ElectrolyteMaterials", NameOfTheCathode_Anode_ElectrolyteMaterials);
            encoder.WriteString("RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials", RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials);
            encoder.WriteDouble("WeightOfTheCathode_Anode_ElectrolyteMaterials", WeightOfTheCathode_Anode_ElectrolyteMaterials);
            encoder.WriteString("NameOfHazardousSubstances", NameOfHazardousSubstances);
            encoder.WriteString("HazardClassesAnd_OrCategoriesOfHazardousSubstances", HazardClassesAnd_OrCategoriesOfHazardousSubstances);
            encoder.WriteString("RelatedIdentifiersOfHazardousSubstances", RelatedIdentifiersOfHazardousSubstances);
            encoder.WriteString("LocationOfHazardousSubstances", LocationOfHazardousSubstances);
            encoder.WriteDouble("ConcentrationRangeOfHazardousSubstances", ConcentrationRangeOfHazardousSubstances);
            encoder.WriteString("ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety", ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            CriticalRawMaterials = decoder.ReadString("CriticalRawMaterials");
            BatteryChemistry = decoder.ReadString("BatteryChemistry");
            NameOfTheCathode_Anode_ElectrolyteMaterials = decoder.ReadString("NameOfTheCathode_Anode_ElectrolyteMaterials");
            RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials = decoder.ReadString("RelatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials");
            WeightOfTheCathode_Anode_ElectrolyteMaterials = decoder.ReadDouble("WeightOfTheCathode_Anode_ElectrolyteMaterials");
            NameOfHazardousSubstances = decoder.ReadString("NameOfHazardousSubstances");
            HazardClassesAnd_OrCategoriesOfHazardousSubstances = decoder.ReadString("HazardClassesAnd_OrCategoriesOfHazardousSubstances");
            RelatedIdentifiersOfHazardousSubstances = decoder.ReadString("RelatedIdentifiersOfHazardousSubstances");
            LocationOfHazardousSubstances = decoder.ReadString("LocationOfHazardousSubstances");
            ConcentrationRangeOfHazardousSubstances = decoder.ReadDouble("ConcentrationRangeOfHazardousSubstances");
            ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety = decoder.ReadString("ImpactOfSubstancesOnTheEnvironment_HumanHealth_Safety");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            BatteryMaterialsAndCompositionDataType value = encodeable as BatteryMaterialsAndCompositionDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_criticalRawMaterials, value.m_criticalRawMaterials)) return false;
            if (!Utils.IsEqual(m_batteryChemistry, value.m_batteryChemistry)) return false;
            if (!Utils.IsEqual(m_nameOfTheCathode_Anode_ElectrolyteMaterials, value.m_nameOfTheCathode_Anode_ElectrolyteMaterials)) return false;
            if (!Utils.IsEqual(m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials, value.m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials)) return false;
            if (!Utils.IsEqual(m_weightOfTheCathode_Anode_ElectrolyteMaterials, value.m_weightOfTheCathode_Anode_ElectrolyteMaterials)) return false;
            if (!Utils.IsEqual(m_nameOfHazardousSubstances, value.m_nameOfHazardousSubstances)) return false;
            if (!Utils.IsEqual(m_hazardClassesAnd_OrCategoriesOfHazardousSubstances, value.m_hazardClassesAnd_OrCategoriesOfHazardousSubstances)) return false;
            if (!Utils.IsEqual(m_relatedIdentifiersOfHazardousSubstances, value.m_relatedIdentifiersOfHazardousSubstances)) return false;
            if (!Utils.IsEqual(m_locationOfHazardousSubstances, value.m_locationOfHazardousSubstances)) return false;
            if (!Utils.IsEqual(m_concentrationRangeOfHazardousSubstances, value.m_concentrationRangeOfHazardousSubstances)) return false;
            if (!Utils.IsEqual(m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety, value.m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (BatteryMaterialsAndCompositionDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            BatteryMaterialsAndCompositionDataType clone = (BatteryMaterialsAndCompositionDataType)base.MemberwiseClone();

            clone.m_criticalRawMaterials = (string)Utils.Clone(this.m_criticalRawMaterials);
            clone.m_batteryChemistry = (string)Utils.Clone(this.m_batteryChemistry);
            clone.m_nameOfTheCathode_Anode_ElectrolyteMaterials = (string)Utils.Clone(this.m_nameOfTheCathode_Anode_ElectrolyteMaterials);
            clone.m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials = (string)Utils.Clone(this.m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials);
            clone.m_weightOfTheCathode_Anode_ElectrolyteMaterials = (double)Utils.Clone(this.m_weightOfTheCathode_Anode_ElectrolyteMaterials);
            clone.m_nameOfHazardousSubstances = (string)Utils.Clone(this.m_nameOfHazardousSubstances);
            clone.m_hazardClassesAnd_OrCategoriesOfHazardousSubstances = (string)Utils.Clone(this.m_hazardClassesAnd_OrCategoriesOfHazardousSubstances);
            clone.m_relatedIdentifiersOfHazardousSubstances = (string)Utils.Clone(this.m_relatedIdentifiersOfHazardousSubstances);
            clone.m_locationOfHazardousSubstances = (string)Utils.Clone(this.m_locationOfHazardousSubstances);
            clone.m_concentrationRangeOfHazardousSubstances = (double)Utils.Clone(this.m_concentrationRangeOfHazardousSubstances);
            clone.m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety = (string)Utils.Clone(this.m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_criticalRawMaterials;
        private string m_batteryChemistry;
        private string m_nameOfTheCathode_Anode_ElectrolyteMaterials;
        private string m_relatedIdentifiersOfTheCathode_Anode_ElectrolyteMaterials;
        private double m_weightOfTheCathode_Anode_ElectrolyteMaterials;
        private string m_nameOfHazardousSubstances;
        private string m_hazardClassesAnd_OrCategoriesOfHazardousSubstances;
        private string m_relatedIdentifiersOfHazardousSubstances;
        private string m_locationOfHazardousSubstances;
        private double m_concentrationRangeOfHazardousSubstances;
        private string m_impactOfSubstancesOnTheEnvironment_HumanHealth_Safety;
        #endregion
    }

    #region BatteryMaterialsAndCompositionDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfBatteryMaterialsAndCompositionDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "BatteryMaterialsAndCompositionDataType")]
    public partial class BatteryMaterialsAndCompositionDataTypeCollection : List<BatteryMaterialsAndCompositionDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public BatteryMaterialsAndCompositionDataTypeCollection() {}

        /// <remarks />
        public BatteryMaterialsAndCompositionDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public BatteryMaterialsAndCompositionDataTypeCollection(IEnumerable<BatteryMaterialsAndCompositionDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator BatteryMaterialsAndCompositionDataTypeCollection(BatteryMaterialsAndCompositionDataType[] values)
        {
            if (values != null)
            {
                return new BatteryMaterialsAndCompositionDataTypeCollection(values);
            }

            return new BatteryMaterialsAndCompositionDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator BatteryMaterialsAndCompositionDataType[](BatteryMaterialsAndCompositionDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (BatteryMaterialsAndCompositionDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            BatteryMaterialsAndCompositionDataTypeCollection clone = new BatteryMaterialsAndCompositionDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((BatteryMaterialsAndCompositionDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region CarbonFootprintDataType Class
    #if (!OPCUA_EXCLUDE_CarbonFootprintDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class CarbonFootprintDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public CarbonFootprintDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_batteryCarbonFootprint = (double)0;
            m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing = (double)0;
            m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction = (double)0;
            m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution = (double)0;
            m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling = (double)0;
            m_carbonFootprintPerformanceClass = null;
            m_webLinkToPublicCarbonFootprintStudy = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "BatteryCarbonFootprint", IsRequired = false, Order = 1)]
        public double BatteryCarbonFootprint
        {
            get { return m_batteryCarbonFootprint;  }
            set { m_batteryCarbonFootprint = value; }
        }

        /// <remarks />
        [DataMember(Name = "ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing", IsRequired = false, Order = 2)]
        public double ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing
        {
            get { return m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing;  }
            set { m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing = value; }
        }

        /// <remarks />
        [DataMember(Name = "ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction", IsRequired = false, Order = 3)]
        public double ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction
        {
            get { return m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction;  }
            set { m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction = value; }
        }

        /// <remarks />
        [DataMember(Name = "ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution", IsRequired = false, Order = 4)]
        public double ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution
        {
            get { return m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution;  }
            set { m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution = value; }
        }

        /// <remarks />
        [DataMember(Name = "ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling", IsRequired = false, Order = 5)]
        public double ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling
        {
            get { return m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling;  }
            set { m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling = value; }
        }

        /// <remarks />
        [DataMember(Name = "CarbonFootprintPerformanceClass", IsRequired = false, Order = 6)]
        public string CarbonFootprintPerformanceClass
        {
            get { return m_carbonFootprintPerformanceClass;  }
            set { m_carbonFootprintPerformanceClass = value; }
        }

        /// <remarks />
        [DataMember(Name = "WebLinkToPublicCarbonFootprintStudy", IsRequired = false, Order = 7)]
        public string WebLinkToPublicCarbonFootprintStudy
        {
            get { return m_webLinkToPublicCarbonFootprintStudy;  }
            set { m_webLinkToPublicCarbonFootprintStudy = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.CarbonFootprintDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.CarbonFootprintDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.CarbonFootprintDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.CarbonFootprintDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteDouble("BatteryCarbonFootprint", BatteryCarbonFootprint);
            encoder.WriteDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing", ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing);
            encoder.WriteDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction", ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction);
            encoder.WriteDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution", ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution);
            encoder.WriteDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling", ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling);
            encoder.WriteString("CarbonFootprintPerformanceClass", CarbonFootprintPerformanceClass);
            encoder.WriteString("WebLinkToPublicCarbonFootprintStudy", WebLinkToPublicCarbonFootprintStudy);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            BatteryCarbonFootprint = decoder.ReadDouble("BatteryCarbonFootprint");
            ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing = decoder.ReadDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing");
            ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction = decoder.ReadDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction");
            ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution = decoder.ReadDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_Distribution");
            ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling = decoder.ReadDouble("ShareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling");
            CarbonFootprintPerformanceClass = decoder.ReadString("CarbonFootprintPerformanceClass");
            WebLinkToPublicCarbonFootprintStudy = decoder.ReadString("WebLinkToPublicCarbonFootprintStudy");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            CarbonFootprintDataType value = encodeable as CarbonFootprintDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_batteryCarbonFootprint, value.m_batteryCarbonFootprint)) return false;
            if (!Utils.IsEqual(m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing, value.m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing)) return false;
            if (!Utils.IsEqual(m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction, value.m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction)) return false;
            if (!Utils.IsEqual(m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution, value.m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution)) return false;
            if (!Utils.IsEqual(m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling, value.m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling)) return false;
            if (!Utils.IsEqual(m_carbonFootprintPerformanceClass, value.m_carbonFootprintPerformanceClass)) return false;
            if (!Utils.IsEqual(m_webLinkToPublicCarbonFootprintStudy, value.m_webLinkToPublicCarbonFootprintStudy)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (CarbonFootprintDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            CarbonFootprintDataType clone = (CarbonFootprintDataType)base.MemberwiseClone();

            clone.m_batteryCarbonFootprint = (double)Utils.Clone(this.m_batteryCarbonFootprint);
            clone.m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing = (double)Utils.Clone(this.m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing);
            clone.m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction = (double)Utils.Clone(this.m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction);
            clone.m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution = (double)Utils.Clone(this.m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution);
            clone.m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling = (double)Utils.Clone(this.m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling);
            clone.m_carbonFootprintPerformanceClass = (string)Utils.Clone(this.m_carbonFootprintPerformanceClass);
            clone.m_webLinkToPublicCarbonFootprintStudy = (string)Utils.Clone(this.m_webLinkToPublicCarbonFootprintStudy);

            return clone;
        }
        #endregion

        #region Private Fields
        private double m_batteryCarbonFootprint;
        private double m_shareOfBatteryCarbonFootprintPerLifecycleStage_RawMaterialAcquisitionAndPre_Processing;
        private double m_shareOfBatteryCarbonFootprintPerLifecycleStage_MainProductProduction;
        private double m_shareOfBatteryCarbonFootprintPerLifecycleStage_Distribution;
        private double m_shareOfBatteryCarbonFootprintPerLifecycleStage_EndOfLifeAndRecycling;
        private string m_carbonFootprintPerformanceClass;
        private string m_webLinkToPublicCarbonFootprintStudy;
        #endregion
    }

    #region CarbonFootprintDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfCarbonFootprintDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "CarbonFootprintDataType")]
    public partial class CarbonFootprintDataTypeCollection : List<CarbonFootprintDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public CarbonFootprintDataTypeCollection() {}

        /// <remarks />
        public CarbonFootprintDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public CarbonFootprintDataTypeCollection(IEnumerable<CarbonFootprintDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator CarbonFootprintDataTypeCollection(CarbonFootprintDataType[] values)
        {
            if (values != null)
            {
                return new CarbonFootprintDataTypeCollection(values);
            }

            return new CarbonFootprintDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator CarbonFootprintDataType[](CarbonFootprintDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (CarbonFootprintDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            CarbonFootprintDataTypeCollection clone = new CarbonFootprintDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((CarbonFootprintDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region CircularityAndResourceEfficiencyDataType Class
    #if (!OPCUA_EXCLUDE_CircularityAndResourceEfficiencyDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class CircularityAndResourceEfficiencyDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public CircularityAndResourceEfficiencyDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_manualForRemovalOfTheBatteryFromTheAppliance = null;
            m_manualForDisassemblyAndDismantlingOfTheBatteryPack = null;
            m_postalAddressOfSourcesForSpareParts = null;
            m_e_MailAddressOfSourcesForSpareParts = null;
            m_webAddressOfSourcesForSpareParts = null;
            m_partNumbersForComponents = null;
            m_extinguishingAgent = null;
            m_safetyMeasures_Instructions = null;
            m_pre_ConsumerRecycledNickelShare = (double)0;
            m_pre_ConsumerRecycledCobaltShare = (double)0;
            m_pre_ConsumerRecycledLithiumShare = (double)0;
            m_pre_ConsumerRecycledLeadShare = (double)0;
            m_post_ConsumerRecycledNickelShare = (double)0;
            m_post_ConsumerRecycledCobaltShare = (double)0;
            m_post_ConsumerRecycledLithiumShare = (double)0;
            m_post_ConsumerRecycledLeadShare = (double)0;
            m_renewableContentShare = (double)0;
            m_roleOfEnd_UsersInContributingToWastePrevention = null;
            m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries = null;
            m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "ManualForRemovalOfTheBatteryFromTheAppliance", IsRequired = false, Order = 1)]
        public string ManualForRemovalOfTheBatteryFromTheAppliance
        {
            get { return m_manualForRemovalOfTheBatteryFromTheAppliance;  }
            set { m_manualForRemovalOfTheBatteryFromTheAppliance = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManualForDisassemblyAndDismantlingOfTheBatteryPack", IsRequired = false, Order = 2)]
        public string ManualForDisassemblyAndDismantlingOfTheBatteryPack
        {
            get { return m_manualForDisassemblyAndDismantlingOfTheBatteryPack;  }
            set { m_manualForDisassemblyAndDismantlingOfTheBatteryPack = value; }
        }

        /// <remarks />
        [DataMember(Name = "PostalAddressOfSourcesForSpareParts", IsRequired = false, Order = 3)]
        public string PostalAddressOfSourcesForSpareParts
        {
            get { return m_postalAddressOfSourcesForSpareParts;  }
            set { m_postalAddressOfSourcesForSpareParts = value; }
        }

        /// <remarks />
        [DataMember(Name = "E_MailAddressOfSourcesForSpareParts", IsRequired = false, Order = 4)]
        public string E_MailAddressOfSourcesForSpareParts
        {
            get { return m_e_MailAddressOfSourcesForSpareParts;  }
            set { m_e_MailAddressOfSourcesForSpareParts = value; }
        }

        /// <remarks />
        [DataMember(Name = "WebAddressOfSourcesForSpareParts", IsRequired = false, Order = 5)]
        public string WebAddressOfSourcesForSpareParts
        {
            get { return m_webAddressOfSourcesForSpareParts;  }
            set { m_webAddressOfSourcesForSpareParts = value; }
        }

        /// <remarks />
        [DataMember(Name = "PartNumbersForComponents", IsRequired = false, Order = 6)]
        public string PartNumbersForComponents
        {
            get { return m_partNumbersForComponents;  }
            set { m_partNumbersForComponents = value; }
        }

        /// <remarks />
        [DataMember(Name = "ExtinguishingAgent", IsRequired = false, Order = 7)]
        public string ExtinguishingAgent
        {
            get { return m_extinguishingAgent;  }
            set { m_extinguishingAgent = value; }
        }

        /// <remarks />
        [DataMember(Name = "SafetyMeasures_Instructions", IsRequired = false, Order = 8)]
        public string SafetyMeasures_Instructions
        {
            get { return m_safetyMeasures_Instructions;  }
            set { m_safetyMeasures_Instructions = value; }
        }

        /// <remarks />
        [DataMember(Name = "Pre_ConsumerRecycledNickelShare", IsRequired = false, Order = 9)]
        public double Pre_ConsumerRecycledNickelShare
        {
            get { return m_pre_ConsumerRecycledNickelShare;  }
            set { m_pre_ConsumerRecycledNickelShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Pre_ConsumerRecycledCobaltShare", IsRequired = false, Order = 10)]
        public double Pre_ConsumerRecycledCobaltShare
        {
            get { return m_pre_ConsumerRecycledCobaltShare;  }
            set { m_pre_ConsumerRecycledCobaltShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Pre_ConsumerRecycledLithiumShare", IsRequired = false, Order = 11)]
        public double Pre_ConsumerRecycledLithiumShare
        {
            get { return m_pre_ConsumerRecycledLithiumShare;  }
            set { m_pre_ConsumerRecycledLithiumShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Pre_ConsumerRecycledLeadShare", IsRequired = false, Order = 12)]
        public double Pre_ConsumerRecycledLeadShare
        {
            get { return m_pre_ConsumerRecycledLeadShare;  }
            set { m_pre_ConsumerRecycledLeadShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Post_ConsumerRecycledNickelShare", IsRequired = false, Order = 13)]
        public double Post_ConsumerRecycledNickelShare
        {
            get { return m_post_ConsumerRecycledNickelShare;  }
            set { m_post_ConsumerRecycledNickelShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Post_ConsumerRecycledCobaltShare", IsRequired = false, Order = 14)]
        public double Post_ConsumerRecycledCobaltShare
        {
            get { return m_post_ConsumerRecycledCobaltShare;  }
            set { m_post_ConsumerRecycledCobaltShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Post_ConsumerRecycledLithiumShare", IsRequired = false, Order = 15)]
        public double Post_ConsumerRecycledLithiumShare
        {
            get { return m_post_ConsumerRecycledLithiumShare;  }
            set { m_post_ConsumerRecycledLithiumShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "Post_ConsumerRecycledLeadShare", IsRequired = false, Order = 16)]
        public double Post_ConsumerRecycledLeadShare
        {
            get { return m_post_ConsumerRecycledLeadShare;  }
            set { m_post_ConsumerRecycledLeadShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "RenewableContentShare", IsRequired = false, Order = 17)]
        public double RenewableContentShare
        {
            get { return m_renewableContentShare;  }
            set { m_renewableContentShare = value; }
        }

        /// <remarks />
        [DataMember(Name = "RoleOfEnd_UsersInContributingToWastePrevention", IsRequired = false, Order = 18)]
        public string RoleOfEnd_UsersInContributingToWastePrevention
        {
            get { return m_roleOfEnd_UsersInContributingToWastePrevention;  }
            set { m_roleOfEnd_UsersInContributingToWastePrevention = value; }
        }

        /// <remarks />
        [DataMember(Name = "RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries", IsRequired = false, Order = 19)]
        public string RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries
        {
            get { return m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries;  }
            set { m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries = value; }
        }

        /// <remarks />
        [DataMember(Name = "InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations", IsRequired = false, Order = 20)]
        public string InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations
        {
            get { return m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations;  }
            set { m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.CircularityAndResourceEfficiencyDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.CircularityAndResourceEfficiencyDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.CircularityAndResourceEfficiencyDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.CircularityAndResourceEfficiencyDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteString("ManualForRemovalOfTheBatteryFromTheAppliance", ManualForRemovalOfTheBatteryFromTheAppliance);
            encoder.WriteString("ManualForDisassemblyAndDismantlingOfTheBatteryPack", ManualForDisassemblyAndDismantlingOfTheBatteryPack);
            encoder.WriteString("PostalAddressOfSourcesForSpareParts", PostalAddressOfSourcesForSpareParts);
            encoder.WriteString("E_MailAddressOfSourcesForSpareParts", E_MailAddressOfSourcesForSpareParts);
            encoder.WriteString("WebAddressOfSourcesForSpareParts", WebAddressOfSourcesForSpareParts);
            encoder.WriteString("PartNumbersForComponents", PartNumbersForComponents);
            encoder.WriteString("ExtinguishingAgent", ExtinguishingAgent);
            encoder.WriteString("SafetyMeasures_Instructions", SafetyMeasures_Instructions);
            encoder.WriteDouble("Pre_ConsumerRecycledNickelShare", Pre_ConsumerRecycledNickelShare);
            encoder.WriteDouble("Pre_ConsumerRecycledCobaltShare", Pre_ConsumerRecycledCobaltShare);
            encoder.WriteDouble("Pre_ConsumerRecycledLithiumShare", Pre_ConsumerRecycledLithiumShare);
            encoder.WriteDouble("Pre_ConsumerRecycledLeadShare", Pre_ConsumerRecycledLeadShare);
            encoder.WriteDouble("Post_ConsumerRecycledNickelShare", Post_ConsumerRecycledNickelShare);
            encoder.WriteDouble("Post_ConsumerRecycledCobaltShare", Post_ConsumerRecycledCobaltShare);
            encoder.WriteDouble("Post_ConsumerRecycledLithiumShare", Post_ConsumerRecycledLithiumShare);
            encoder.WriteDouble("Post_ConsumerRecycledLeadShare", Post_ConsumerRecycledLeadShare);
            encoder.WriteDouble("RenewableContentShare", RenewableContentShare);
            encoder.WriteString("RoleOfEnd_UsersInContributingToWastePrevention", RoleOfEnd_UsersInContributingToWastePrevention);
            encoder.WriteString("RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries", RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries);
            encoder.WriteString("InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations", InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            ManualForRemovalOfTheBatteryFromTheAppliance = decoder.ReadString("ManualForRemovalOfTheBatteryFromTheAppliance");
            ManualForDisassemblyAndDismantlingOfTheBatteryPack = decoder.ReadString("ManualForDisassemblyAndDismantlingOfTheBatteryPack");
            PostalAddressOfSourcesForSpareParts = decoder.ReadString("PostalAddressOfSourcesForSpareParts");
            E_MailAddressOfSourcesForSpareParts = decoder.ReadString("E_MailAddressOfSourcesForSpareParts");
            WebAddressOfSourcesForSpareParts = decoder.ReadString("WebAddressOfSourcesForSpareParts");
            PartNumbersForComponents = decoder.ReadString("PartNumbersForComponents");
            ExtinguishingAgent = decoder.ReadString("ExtinguishingAgent");
            SafetyMeasures_Instructions = decoder.ReadString("SafetyMeasures_Instructions");
            Pre_ConsumerRecycledNickelShare = decoder.ReadDouble("Pre_ConsumerRecycledNickelShare");
            Pre_ConsumerRecycledCobaltShare = decoder.ReadDouble("Pre_ConsumerRecycledCobaltShare");
            Pre_ConsumerRecycledLithiumShare = decoder.ReadDouble("Pre_ConsumerRecycledLithiumShare");
            Pre_ConsumerRecycledLeadShare = decoder.ReadDouble("Pre_ConsumerRecycledLeadShare");
            Post_ConsumerRecycledNickelShare = decoder.ReadDouble("Post_ConsumerRecycledNickelShare");
            Post_ConsumerRecycledCobaltShare = decoder.ReadDouble("Post_ConsumerRecycledCobaltShare");
            Post_ConsumerRecycledLithiumShare = decoder.ReadDouble("Post_ConsumerRecycledLithiumShare");
            Post_ConsumerRecycledLeadShare = decoder.ReadDouble("Post_ConsumerRecycledLeadShare");
            RenewableContentShare = decoder.ReadDouble("RenewableContentShare");
            RoleOfEnd_UsersInContributingToWastePrevention = decoder.ReadString("RoleOfEnd_UsersInContributingToWastePrevention");
            RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries = decoder.ReadString("RoleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries");
            InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations = decoder.ReadString("InformationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            CircularityAndResourceEfficiencyDataType value = encodeable as CircularityAndResourceEfficiencyDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_manualForRemovalOfTheBatteryFromTheAppliance, value.m_manualForRemovalOfTheBatteryFromTheAppliance)) return false;
            if (!Utils.IsEqual(m_manualForDisassemblyAndDismantlingOfTheBatteryPack, value.m_manualForDisassemblyAndDismantlingOfTheBatteryPack)) return false;
            if (!Utils.IsEqual(m_postalAddressOfSourcesForSpareParts, value.m_postalAddressOfSourcesForSpareParts)) return false;
            if (!Utils.IsEqual(m_e_MailAddressOfSourcesForSpareParts, value.m_e_MailAddressOfSourcesForSpareParts)) return false;
            if (!Utils.IsEqual(m_webAddressOfSourcesForSpareParts, value.m_webAddressOfSourcesForSpareParts)) return false;
            if (!Utils.IsEqual(m_partNumbersForComponents, value.m_partNumbersForComponents)) return false;
            if (!Utils.IsEqual(m_extinguishingAgent, value.m_extinguishingAgent)) return false;
            if (!Utils.IsEqual(m_safetyMeasures_Instructions, value.m_safetyMeasures_Instructions)) return false;
            if (!Utils.IsEqual(m_pre_ConsumerRecycledNickelShare, value.m_pre_ConsumerRecycledNickelShare)) return false;
            if (!Utils.IsEqual(m_pre_ConsumerRecycledCobaltShare, value.m_pre_ConsumerRecycledCobaltShare)) return false;
            if (!Utils.IsEqual(m_pre_ConsumerRecycledLithiumShare, value.m_pre_ConsumerRecycledLithiumShare)) return false;
            if (!Utils.IsEqual(m_pre_ConsumerRecycledLeadShare, value.m_pre_ConsumerRecycledLeadShare)) return false;
            if (!Utils.IsEqual(m_post_ConsumerRecycledNickelShare, value.m_post_ConsumerRecycledNickelShare)) return false;
            if (!Utils.IsEqual(m_post_ConsumerRecycledCobaltShare, value.m_post_ConsumerRecycledCobaltShare)) return false;
            if (!Utils.IsEqual(m_post_ConsumerRecycledLithiumShare, value.m_post_ConsumerRecycledLithiumShare)) return false;
            if (!Utils.IsEqual(m_post_ConsumerRecycledLeadShare, value.m_post_ConsumerRecycledLeadShare)) return false;
            if (!Utils.IsEqual(m_renewableContentShare, value.m_renewableContentShare)) return false;
            if (!Utils.IsEqual(m_roleOfEnd_UsersInContributingToWastePrevention, value.m_roleOfEnd_UsersInContributingToWastePrevention)) return false;
            if (!Utils.IsEqual(m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries, value.m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries)) return false;
            if (!Utils.IsEqual(m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations, value.m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (CircularityAndResourceEfficiencyDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            CircularityAndResourceEfficiencyDataType clone = (CircularityAndResourceEfficiencyDataType)base.MemberwiseClone();

            clone.m_manualForRemovalOfTheBatteryFromTheAppliance = (string)Utils.Clone(this.m_manualForRemovalOfTheBatteryFromTheAppliance);
            clone.m_manualForDisassemblyAndDismantlingOfTheBatteryPack = (string)Utils.Clone(this.m_manualForDisassemblyAndDismantlingOfTheBatteryPack);
            clone.m_postalAddressOfSourcesForSpareParts = (string)Utils.Clone(this.m_postalAddressOfSourcesForSpareParts);
            clone.m_e_MailAddressOfSourcesForSpareParts = (string)Utils.Clone(this.m_e_MailAddressOfSourcesForSpareParts);
            clone.m_webAddressOfSourcesForSpareParts = (string)Utils.Clone(this.m_webAddressOfSourcesForSpareParts);
            clone.m_partNumbersForComponents = (string)Utils.Clone(this.m_partNumbersForComponents);
            clone.m_extinguishingAgent = (string)Utils.Clone(this.m_extinguishingAgent);
            clone.m_safetyMeasures_Instructions = (string)Utils.Clone(this.m_safetyMeasures_Instructions);
            clone.m_pre_ConsumerRecycledNickelShare = (double)Utils.Clone(this.m_pre_ConsumerRecycledNickelShare);
            clone.m_pre_ConsumerRecycledCobaltShare = (double)Utils.Clone(this.m_pre_ConsumerRecycledCobaltShare);
            clone.m_pre_ConsumerRecycledLithiumShare = (double)Utils.Clone(this.m_pre_ConsumerRecycledLithiumShare);
            clone.m_pre_ConsumerRecycledLeadShare = (double)Utils.Clone(this.m_pre_ConsumerRecycledLeadShare);
            clone.m_post_ConsumerRecycledNickelShare = (double)Utils.Clone(this.m_post_ConsumerRecycledNickelShare);
            clone.m_post_ConsumerRecycledCobaltShare = (double)Utils.Clone(this.m_post_ConsumerRecycledCobaltShare);
            clone.m_post_ConsumerRecycledLithiumShare = (double)Utils.Clone(this.m_post_ConsumerRecycledLithiumShare);
            clone.m_post_ConsumerRecycledLeadShare = (double)Utils.Clone(this.m_post_ConsumerRecycledLeadShare);
            clone.m_renewableContentShare = (double)Utils.Clone(this.m_renewableContentShare);
            clone.m_roleOfEnd_UsersInContributingToWastePrevention = (string)Utils.Clone(this.m_roleOfEnd_UsersInContributingToWastePrevention);
            clone.m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries = (string)Utils.Clone(this.m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries);
            clone.m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations = (string)Utils.Clone(this.m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_manualForRemovalOfTheBatteryFromTheAppliance;
        private string m_manualForDisassemblyAndDismantlingOfTheBatteryPack;
        private string m_postalAddressOfSourcesForSpareParts;
        private string m_e_MailAddressOfSourcesForSpareParts;
        private string m_webAddressOfSourcesForSpareParts;
        private string m_partNumbersForComponents;
        private string m_extinguishingAgent;
        private string m_safetyMeasures_Instructions;
        private double m_pre_ConsumerRecycledNickelShare;
        private double m_pre_ConsumerRecycledCobaltShare;
        private double m_pre_ConsumerRecycledLithiumShare;
        private double m_pre_ConsumerRecycledLeadShare;
        private double m_post_ConsumerRecycledNickelShare;
        private double m_post_ConsumerRecycledCobaltShare;
        private double m_post_ConsumerRecycledLithiumShare;
        private double m_post_ConsumerRecycledLeadShare;
        private double m_renewableContentShare;
        private string m_roleOfEnd_UsersInContributingToWastePrevention;
        private string m_roleOfEnd_UsersInContributingToTheSeparateCollectionOfWasteBatteries;
        private string m_informationOnSeparateCollection_TakeBack_CollectionPointsAndPreparingForRe_Use_PreparingForRepurposingAndRecyclingOperations;
        #endregion
    }

    #region CircularityAndResourceEfficiencyDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfCircularityAndResourceEfficiencyDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "CircularityAndResourceEfficiencyDataType")]
    public partial class CircularityAndResourceEfficiencyDataTypeCollection : List<CircularityAndResourceEfficiencyDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public CircularityAndResourceEfficiencyDataTypeCollection() {}

        /// <remarks />
        public CircularityAndResourceEfficiencyDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public CircularityAndResourceEfficiencyDataTypeCollection(IEnumerable<CircularityAndResourceEfficiencyDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator CircularityAndResourceEfficiencyDataTypeCollection(CircularityAndResourceEfficiencyDataType[] values)
        {
            if (values != null)
            {
                return new CircularityAndResourceEfficiencyDataTypeCollection(values);
            }

            return new CircularityAndResourceEfficiencyDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator CircularityAndResourceEfficiencyDataType[](CircularityAndResourceEfficiencyDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (CircularityAndResourceEfficiencyDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            CircularityAndResourceEfficiencyDataTypeCollection clone = new CircularityAndResourceEfficiencyDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((CircularityAndResourceEfficiencyDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region Compliance_LabelsAndCertificationsDataType Class
    #if (!OPCUA_EXCLUDE_Compliance_LabelsAndCertificationsDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class Compliance_LabelsAndCertificationsDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public Compliance_LabelsAndCertificationsDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_resultsOfTestsReports = null;
            m_separateCollectionSymbol = null;
            m_meaningOfLabelsAndSymbols = null;
            m_cadmiumAndLeadSymbols = null;
            m_eUDeclarationOfConformity = null;
            m_iDOfEUDeclarationOfConformity = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "ResultsOfTestsReports", IsRequired = false, Order = 1)]
        public string ResultsOfTestsReports
        {
            get { return m_resultsOfTestsReports;  }
            set { m_resultsOfTestsReports = value; }
        }

        /// <remarks />
        [DataMember(Name = "SeparateCollectionSymbol", IsRequired = false, Order = 2)]
        public string SeparateCollectionSymbol
        {
            get { return m_separateCollectionSymbol;  }
            set { m_separateCollectionSymbol = value; }
        }

        /// <remarks />
        [DataMember(Name = "MeaningOfLabelsAndSymbols", IsRequired = false, Order = 3)]
        public string MeaningOfLabelsAndSymbols
        {
            get { return m_meaningOfLabelsAndSymbols;  }
            set { m_meaningOfLabelsAndSymbols = value; }
        }

        /// <remarks />
        [DataMember(Name = "CadmiumAndLeadSymbols", IsRequired = false, Order = 4)]
        public string CadmiumAndLeadSymbols
        {
            get { return m_cadmiumAndLeadSymbols;  }
            set { m_cadmiumAndLeadSymbols = value; }
        }

        /// <remarks />
        [DataMember(Name = "EUDeclarationOfConformity", IsRequired = false, Order = 5)]
        public string EUDeclarationOfConformity
        {
            get { return m_eUDeclarationOfConformity;  }
            set { m_eUDeclarationOfConformity = value; }
        }

        /// <remarks />
        [DataMember(Name = "IDOfEUDeclarationOfConformity", IsRequired = false, Order = 6)]
        public string IDOfEUDeclarationOfConformity
        {
            get { return m_iDOfEUDeclarationOfConformity;  }
            set { m_iDOfEUDeclarationOfConformity = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.Compliance_LabelsAndCertificationsDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.Compliance_LabelsAndCertificationsDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.Compliance_LabelsAndCertificationsDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.Compliance_LabelsAndCertificationsDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteString("ResultsOfTestsReports", ResultsOfTestsReports);
            encoder.WriteString("SeparateCollectionSymbol", SeparateCollectionSymbol);
            encoder.WriteString("MeaningOfLabelsAndSymbols", MeaningOfLabelsAndSymbols);
            encoder.WriteString("CadmiumAndLeadSymbols", CadmiumAndLeadSymbols);
            encoder.WriteString("EUDeclarationOfConformity", EUDeclarationOfConformity);
            encoder.WriteString("IDOfEUDeclarationOfConformity", IDOfEUDeclarationOfConformity);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            ResultsOfTestsReports = decoder.ReadString("ResultsOfTestsReports");
            SeparateCollectionSymbol = decoder.ReadString("SeparateCollectionSymbol");
            MeaningOfLabelsAndSymbols = decoder.ReadString("MeaningOfLabelsAndSymbols");
            CadmiumAndLeadSymbols = decoder.ReadString("CadmiumAndLeadSymbols");
            EUDeclarationOfConformity = decoder.ReadString("EUDeclarationOfConformity");
            IDOfEUDeclarationOfConformity = decoder.ReadString("IDOfEUDeclarationOfConformity");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            Compliance_LabelsAndCertificationsDataType value = encodeable as Compliance_LabelsAndCertificationsDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_resultsOfTestsReports, value.m_resultsOfTestsReports)) return false;
            if (!Utils.IsEqual(m_separateCollectionSymbol, value.m_separateCollectionSymbol)) return false;
            if (!Utils.IsEqual(m_meaningOfLabelsAndSymbols, value.m_meaningOfLabelsAndSymbols)) return false;
            if (!Utils.IsEqual(m_cadmiumAndLeadSymbols, value.m_cadmiumAndLeadSymbols)) return false;
            if (!Utils.IsEqual(m_eUDeclarationOfConformity, value.m_eUDeclarationOfConformity)) return false;
            if (!Utils.IsEqual(m_iDOfEUDeclarationOfConformity, value.m_iDOfEUDeclarationOfConformity)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (Compliance_LabelsAndCertificationsDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            Compliance_LabelsAndCertificationsDataType clone = (Compliance_LabelsAndCertificationsDataType)base.MemberwiseClone();

            clone.m_resultsOfTestsReports = (string)Utils.Clone(this.m_resultsOfTestsReports);
            clone.m_separateCollectionSymbol = (string)Utils.Clone(this.m_separateCollectionSymbol);
            clone.m_meaningOfLabelsAndSymbols = (string)Utils.Clone(this.m_meaningOfLabelsAndSymbols);
            clone.m_cadmiumAndLeadSymbols = (string)Utils.Clone(this.m_cadmiumAndLeadSymbols);
            clone.m_eUDeclarationOfConformity = (string)Utils.Clone(this.m_eUDeclarationOfConformity);
            clone.m_iDOfEUDeclarationOfConformity = (string)Utils.Clone(this.m_iDOfEUDeclarationOfConformity);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_resultsOfTestsReports;
        private string m_separateCollectionSymbol;
        private string m_meaningOfLabelsAndSymbols;
        private string m_cadmiumAndLeadSymbols;
        private string m_eUDeclarationOfConformity;
        private string m_iDOfEUDeclarationOfConformity;
        #endregion
    }

    #region Compliance_LabelsAndCertificationsDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfCompliance_LabelsAndCertificationsDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "Compliance_LabelsAndCertificationsDataType")]
    public partial class Compliance_LabelsAndCertificationsDataTypeCollection : List<Compliance_LabelsAndCertificationsDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public Compliance_LabelsAndCertificationsDataTypeCollection() {}

        /// <remarks />
        public Compliance_LabelsAndCertificationsDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public Compliance_LabelsAndCertificationsDataTypeCollection(IEnumerable<Compliance_LabelsAndCertificationsDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator Compliance_LabelsAndCertificationsDataTypeCollection(Compliance_LabelsAndCertificationsDataType[] values)
        {
            if (values != null)
            {
                return new Compliance_LabelsAndCertificationsDataTypeCollection(values);
            }

            return new Compliance_LabelsAndCertificationsDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator Compliance_LabelsAndCertificationsDataType[](Compliance_LabelsAndCertificationsDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (Compliance_LabelsAndCertificationsDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            Compliance_LabelsAndCertificationsDataTypeCollection clone = new Compliance_LabelsAndCertificationsDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((Compliance_LabelsAndCertificationsDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region GeneralBatteryAndManufacturerInformationDataType Class
    #if (!OPCUA_EXCLUDE_GeneralBatteryAndManufacturerInformationDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class GeneralBatteryAndManufacturerInformationDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public GeneralBatteryAndManufacturerInformationDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_batteryUniqueIdentifier = null;
            m_manufacturersIdentification = null;
            m_manufacturingDate = DateTime.MinValue;
            m_manufacturingPlace = null;
            m_batteryCategory = null;
            m_batteryWeight = (double)0;
            m_batteryStatus = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "BatteryUniqueIdentifier", IsRequired = false, Order = 1)]
        public string BatteryUniqueIdentifier
        {
            get { return m_batteryUniqueIdentifier;  }
            set { m_batteryUniqueIdentifier = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturersIdentification", IsRequired = false, Order = 2)]
        public string ManufacturersIdentification
        {
            get { return m_manufacturersIdentification;  }
            set { m_manufacturersIdentification = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturingDate", IsRequired = false, Order = 3)]
        public DateTime ManufacturingDate
        {
            get { return m_manufacturingDate;  }
            set { m_manufacturingDate = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturingPlace", IsRequired = false, Order = 4)]
        public string ManufacturingPlace
        {
            get { return m_manufacturingPlace;  }
            set { m_manufacturingPlace = value; }
        }

        /// <remarks />
        [DataMember(Name = "BatteryCategory", IsRequired = false, Order = 5)]
        public string BatteryCategory
        {
            get { return m_batteryCategory;  }
            set { m_batteryCategory = value; }
        }

        /// <remarks />
        [DataMember(Name = "BatteryWeight", IsRequired = false, Order = 6)]
        public double BatteryWeight
        {
            get { return m_batteryWeight;  }
            set { m_batteryWeight = value; }
        }

        /// <remarks />
        [DataMember(Name = "BatteryStatus", IsRequired = false, Order = 7)]
        public string BatteryStatus
        {
            get { return m_batteryStatus;  }
            set { m_batteryStatus = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.GeneralBatteryAndManufacturerInformationDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.GeneralBatteryAndManufacturerInformationDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.GeneralBatteryAndManufacturerInformationDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.GeneralBatteryAndManufacturerInformationDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteString("BatteryUniqueIdentifier", BatteryUniqueIdentifier);
            encoder.WriteString("ManufacturersIdentification", ManufacturersIdentification);
            encoder.WriteDateTime("ManufacturingDate", ManufacturingDate);
            encoder.WriteString("ManufacturingPlace", ManufacturingPlace);
            encoder.WriteString("BatteryCategory", BatteryCategory);
            encoder.WriteDouble("BatteryWeight", BatteryWeight);
            encoder.WriteString("BatteryStatus", BatteryStatus);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            BatteryUniqueIdentifier = decoder.ReadString("BatteryUniqueIdentifier");
            ManufacturersIdentification = decoder.ReadString("ManufacturersIdentification");
            ManufacturingDate = decoder.ReadDateTime("ManufacturingDate");
            ManufacturingPlace = decoder.ReadString("ManufacturingPlace");
            BatteryCategory = decoder.ReadString("BatteryCategory");
            BatteryWeight = decoder.ReadDouble("BatteryWeight");
            BatteryStatus = decoder.ReadString("BatteryStatus");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            GeneralBatteryAndManufacturerInformationDataType value = encodeable as GeneralBatteryAndManufacturerInformationDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_batteryUniqueIdentifier, value.m_batteryUniqueIdentifier)) return false;
            if (!Utils.IsEqual(m_manufacturersIdentification, value.m_manufacturersIdentification)) return false;
            if (!Utils.IsEqual(m_manufacturingDate, value.m_manufacturingDate)) return false;
            if (!Utils.IsEqual(m_manufacturingPlace, value.m_manufacturingPlace)) return false;
            if (!Utils.IsEqual(m_batteryCategory, value.m_batteryCategory)) return false;
            if (!Utils.IsEqual(m_batteryWeight, value.m_batteryWeight)) return false;
            if (!Utils.IsEqual(m_batteryStatus, value.m_batteryStatus)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (GeneralBatteryAndManufacturerInformationDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            GeneralBatteryAndManufacturerInformationDataType clone = (GeneralBatteryAndManufacturerInformationDataType)base.MemberwiseClone();

            clone.m_batteryUniqueIdentifier = (string)Utils.Clone(this.m_batteryUniqueIdentifier);
            clone.m_manufacturersIdentification = (string)Utils.Clone(this.m_manufacturersIdentification);
            clone.m_manufacturingDate = (DateTime)Utils.Clone(this.m_manufacturingDate);
            clone.m_manufacturingPlace = (string)Utils.Clone(this.m_manufacturingPlace);
            clone.m_batteryCategory = (string)Utils.Clone(this.m_batteryCategory);
            clone.m_batteryWeight = (double)Utils.Clone(this.m_batteryWeight);
            clone.m_batteryStatus = (string)Utils.Clone(this.m_batteryStatus);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_batteryUniqueIdentifier;
        private string m_manufacturersIdentification;
        private DateTime m_manufacturingDate;
        private string m_manufacturingPlace;
        private string m_batteryCategory;
        private double m_batteryWeight;
        private string m_batteryStatus;
        #endregion
    }

    #region GeneralBatteryAndManufacturerInformationDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfGeneralBatteryAndManufacturerInformationDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "GeneralBatteryAndManufacturerInformationDataType")]
    public partial class GeneralBatteryAndManufacturerInformationDataTypeCollection : List<GeneralBatteryAndManufacturerInformationDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public GeneralBatteryAndManufacturerInformationDataTypeCollection() {}

        /// <remarks />
        public GeneralBatteryAndManufacturerInformationDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public GeneralBatteryAndManufacturerInformationDataTypeCollection(IEnumerable<GeneralBatteryAndManufacturerInformationDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator GeneralBatteryAndManufacturerInformationDataTypeCollection(GeneralBatteryAndManufacturerInformationDataType[] values)
        {
            if (values != null)
            {
                return new GeneralBatteryAndManufacturerInformationDataTypeCollection(values);
            }

            return new GeneralBatteryAndManufacturerInformationDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator GeneralBatteryAndManufacturerInformationDataType[](GeneralBatteryAndManufacturerInformationDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (GeneralBatteryAndManufacturerInformationDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            GeneralBatteryAndManufacturerInformationDataTypeCollection clone = new GeneralBatteryAndManufacturerInformationDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((GeneralBatteryAndManufacturerInformationDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region PerformanceAndDurabilityDataType Class
    #if (!OPCUA_EXCLUDE_PerformanceAndDurabilityDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class PerformanceAndDurabilityDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public PerformanceAndDurabilityDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary = (long)0;
            m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary = (long)0;
            m_informationOnAccidents = (long)0;
            m_numberOfDeepDischargeEvents = (long)0;
            m_numberOfOverchargeEvents = (long)0;
            m_certifiedUsableBatteryEnergy_UBEcertified = (long)0;
            m_remainingUsableBatteryEnergy_UBEmeasured = (long)0;
            m_stateOfCertifiedEnergy_SOCE = (long)0;
            m_initialSelf_DischargingRate = (double)0;
            m_currentSelf_DischargingRate = (double)0;
            m_evolutionOfSelf_DischargingRates = (long)0;
            m_ratedCapacity = (long)0;
            m_remainingCapacity = (long)0;
            m_capacityFade = (long)0;
            m_stateOfCharge_SoC = (long)0;
            m_nominalVoltage = (long)0;
            m_minimumVoltage = (long)0;
            m_maximumVoltage = (long)0;
            m_originalPowerCapability = (long)0;
            m_remainingPowerCapability = (long)0;
            m_powerCapabilityFade = (long)0;
            m_maximumPermittedBatteryPower = (long)0;
            m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh = (long)0;
            m_initialRoundTripEnergyEfficiency = (long)0;
            m_roundTripEnergyEfficiencyAt50_OfCycleLife = (long)0;
            m_remainingRoundTripEnergyEfficiency = (long)0;
            m_roundTripEnergyEfficiencyFade = (long)0;
            m_initialInternalResistanceOnBatteryCellLevel = (long)0;
            m_currentInternalResistanceOnBatteryCellLevel = (long)0;
            m_internalResistanceIncreaseOnBatteryCellLevel = (long)0;
            m_initialInternalResistanceOnBatteryPackLevel = (long)0;
            m_currentInternalResistanceOnBatteryPackLevel = (long)0;
            m_initialInternalResistanceOnBatteryModuleLevel = (long)0;
            m_currentInternalResistanceOnBatteryModuleLevel = (long)0;
            m_internalResistanceIncreaseOnBatteryModuleLevel = (long)0;
            m_expectedLifetime_NumberOfCharge_DischargeCycles = (long)0;
            m_numberOf_Full_Charge_DischargeCycles = (long)0;
            m_cycle_LifeReferenceTest = null;
            m_c_RateOfRelevantCycle_LifeTest = (double)0;
            m_energyThroughput = (double)0;
            m_capacityThroughput = (double)0;
            m_capacityThresholdForExhaustion = (long)0;
            m_sOCEThresholdForExhaustion = (long)0;
            m_warrantyPeriodOfTheBattery = (long)0;
            m_dateOfPuttingTheBatteryIntoService = DateTime.MinValue;
            m_temperatureRangeIdleState_LowerBoundary = (long)0;
            m_temperatureRangeIdleState_UpperBoundary = (long)0;
            m_timeSpentInExtremeTemperaturesAboveBoundary = (long)0;
            m_timeSpentInExtremeTemperaturesBelowBoundary = (long)0;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "TimeSpentChargingDuringExtremeTemperaturesAboveBoundary", IsRequired = false, Order = 1)]
        public long TimeSpentChargingDuringExtremeTemperaturesAboveBoundary
        {
            get { return m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary;  }
            set { m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary = value; }
        }

        /// <remarks />
        [DataMember(Name = "TimeSpentChargingDuringExtremeTemperaturesBelowBoundary", IsRequired = false, Order = 2)]
        public long TimeSpentChargingDuringExtremeTemperaturesBelowBoundary
        {
            get { return m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary;  }
            set { m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary = value; }
        }

        /// <remarks />
        [DataMember(Name = "InformationOnAccidents", IsRequired = false, Order = 3)]
        public long InformationOnAccidents
        {
            get { return m_informationOnAccidents;  }
            set { m_informationOnAccidents = value; }
        }

        /// <remarks />
        [DataMember(Name = "NumberOfDeepDischargeEvents", IsRequired = false, Order = 4)]
        public long NumberOfDeepDischargeEvents
        {
            get { return m_numberOfDeepDischargeEvents;  }
            set { m_numberOfDeepDischargeEvents = value; }
        }

        /// <remarks />
        [DataMember(Name = "NumberOfOverchargeEvents", IsRequired = false, Order = 5)]
        public long NumberOfOverchargeEvents
        {
            get { return m_numberOfOverchargeEvents;  }
            set { m_numberOfOverchargeEvents = value; }
        }

        /// <remarks />
        [DataMember(Name = "CertifiedUsableBatteryEnergy_UBEcertified", IsRequired = false, Order = 6)]
        public long CertifiedUsableBatteryEnergy_UBEcertified
        {
            get { return m_certifiedUsableBatteryEnergy_UBEcertified;  }
            set { m_certifiedUsableBatteryEnergy_UBEcertified = value; }
        }

        /// <remarks />
        [DataMember(Name = "RemainingUsableBatteryEnergy_UBEmeasured", IsRequired = false, Order = 7)]
        public long RemainingUsableBatteryEnergy_UBEmeasured
        {
            get { return m_remainingUsableBatteryEnergy_UBEmeasured;  }
            set { m_remainingUsableBatteryEnergy_UBEmeasured = value; }
        }

        /// <remarks />
        [DataMember(Name = "StateOfCertifiedEnergy_SOCE", IsRequired = false, Order = 8)]
        public long StateOfCertifiedEnergy_SOCE
        {
            get { return m_stateOfCertifiedEnergy_SOCE;  }
            set { m_stateOfCertifiedEnergy_SOCE = value; }
        }

        /// <remarks />
        [DataMember(Name = "InitialSelf_DischargingRate", IsRequired = false, Order = 9)]
        public double InitialSelf_DischargingRate
        {
            get { return m_initialSelf_DischargingRate;  }
            set { m_initialSelf_DischargingRate = value; }
        }

        /// <remarks />
        [DataMember(Name = "CurrentSelf_DischargingRate", IsRequired = false, Order = 10)]
        public double CurrentSelf_DischargingRate
        {
            get { return m_currentSelf_DischargingRate;  }
            set { m_currentSelf_DischargingRate = value; }
        }

        /// <remarks />
        [DataMember(Name = "EvolutionOfSelf_DischargingRates", IsRequired = false, Order = 11)]
        public long EvolutionOfSelf_DischargingRates
        {
            get { return m_evolutionOfSelf_DischargingRates;  }
            set { m_evolutionOfSelf_DischargingRates = value; }
        }

        /// <remarks />
        [DataMember(Name = "RatedCapacity", IsRequired = false, Order = 12)]
        public long RatedCapacity
        {
            get { return m_ratedCapacity;  }
            set { m_ratedCapacity = value; }
        }

        /// <remarks />
        [DataMember(Name = "RemainingCapacity", IsRequired = false, Order = 13)]
        public long RemainingCapacity
        {
            get { return m_remainingCapacity;  }
            set { m_remainingCapacity = value; }
        }

        /// <remarks />
        [DataMember(Name = "CapacityFade", IsRequired = false, Order = 14)]
        public long CapacityFade
        {
            get { return m_capacityFade;  }
            set { m_capacityFade = value; }
        }

        /// <remarks />
        [DataMember(Name = "StateOfCharge_SoC", IsRequired = false, Order = 15)]
        public long StateOfCharge_SoC
        {
            get { return m_stateOfCharge_SoC;  }
            set { m_stateOfCharge_SoC = value; }
        }

        /// <remarks />
        [DataMember(Name = "NominalVoltage", IsRequired = false, Order = 16)]
        public long NominalVoltage
        {
            get { return m_nominalVoltage;  }
            set { m_nominalVoltage = value; }
        }

        /// <remarks />
        [DataMember(Name = "MinimumVoltage", IsRequired = false, Order = 17)]
        public long MinimumVoltage
        {
            get { return m_minimumVoltage;  }
            set { m_minimumVoltage = value; }
        }

        /// <remarks />
        [DataMember(Name = "MaximumVoltage", IsRequired = false, Order = 18)]
        public long MaximumVoltage
        {
            get { return m_maximumVoltage;  }
            set { m_maximumVoltage = value; }
        }

        /// <remarks />
        [DataMember(Name = "OriginalPowerCapability", IsRequired = false, Order = 19)]
        public long OriginalPowerCapability
        {
            get { return m_originalPowerCapability;  }
            set { m_originalPowerCapability = value; }
        }

        /// <remarks />
        [DataMember(Name = "RemainingPowerCapability", IsRequired = false, Order = 20)]
        public long RemainingPowerCapability
        {
            get { return m_remainingPowerCapability;  }
            set { m_remainingPowerCapability = value; }
        }

        /// <remarks />
        [DataMember(Name = "PowerCapabilityFade", IsRequired = false, Order = 21)]
        public long PowerCapabilityFade
        {
            get { return m_powerCapabilityFade;  }
            set { m_powerCapabilityFade = value; }
        }

        /// <remarks />
        [DataMember(Name = "MaximumPermittedBatteryPower", IsRequired = false, Order = 22)]
        public long MaximumPermittedBatteryPower
        {
            get { return m_maximumPermittedBatteryPower;  }
            set { m_maximumPermittedBatteryPower = value; }
        }

        /// <remarks />
        [DataMember(Name = "RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh", IsRequired = false, Order = 23)]
        public long RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh
        {
            get { return m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh;  }
            set { m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh = value; }
        }

        /// <remarks />
        [DataMember(Name = "InitialRoundTripEnergyEfficiency", IsRequired = false, Order = 24)]
        public long InitialRoundTripEnergyEfficiency
        {
            get { return m_initialRoundTripEnergyEfficiency;  }
            set { m_initialRoundTripEnergyEfficiency = value; }
        }

        /// <remarks />
        [DataMember(Name = "RoundTripEnergyEfficiencyAt50_OfCycleLife", IsRequired = false, Order = 25)]
        public long RoundTripEnergyEfficiencyAt50_OfCycleLife
        {
            get { return m_roundTripEnergyEfficiencyAt50_OfCycleLife;  }
            set { m_roundTripEnergyEfficiencyAt50_OfCycleLife = value; }
        }

        /// <remarks />
        [DataMember(Name = "RemainingRoundTripEnergyEfficiency", IsRequired = false, Order = 26)]
        public long RemainingRoundTripEnergyEfficiency
        {
            get { return m_remainingRoundTripEnergyEfficiency;  }
            set { m_remainingRoundTripEnergyEfficiency = value; }
        }

        /// <remarks />
        [DataMember(Name = "RoundTripEnergyEfficiencyFade", IsRequired = false, Order = 27)]
        public long RoundTripEnergyEfficiencyFade
        {
            get { return m_roundTripEnergyEfficiencyFade;  }
            set { m_roundTripEnergyEfficiencyFade = value; }
        }

        /// <remarks />
        [DataMember(Name = "InitialInternalResistanceOnBatteryCellLevel", IsRequired = false, Order = 28)]
        public long InitialInternalResistanceOnBatteryCellLevel
        {
            get { return m_initialInternalResistanceOnBatteryCellLevel;  }
            set { m_initialInternalResistanceOnBatteryCellLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "CurrentInternalResistanceOnBatteryCellLevel", IsRequired = false, Order = 29)]
        public long CurrentInternalResistanceOnBatteryCellLevel
        {
            get { return m_currentInternalResistanceOnBatteryCellLevel;  }
            set { m_currentInternalResistanceOnBatteryCellLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "InternalResistanceIncreaseOnBatteryCellLevel", IsRequired = false, Order = 30)]
        public long InternalResistanceIncreaseOnBatteryCellLevel
        {
            get { return m_internalResistanceIncreaseOnBatteryCellLevel;  }
            set { m_internalResistanceIncreaseOnBatteryCellLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "InitialInternalResistanceOnBatteryPackLevel", IsRequired = false, Order = 31)]
        public long InitialInternalResistanceOnBatteryPackLevel
        {
            get { return m_initialInternalResistanceOnBatteryPackLevel;  }
            set { m_initialInternalResistanceOnBatteryPackLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "CurrentInternalResistanceOnBatteryPackLevel", IsRequired = false, Order = 32)]
        public long CurrentInternalResistanceOnBatteryPackLevel
        {
            get { return m_currentInternalResistanceOnBatteryPackLevel;  }
            set { m_currentInternalResistanceOnBatteryPackLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "InitialInternalResistanceOnBatteryModuleLevel", IsRequired = false, Order = 33)]
        public long InitialInternalResistanceOnBatteryModuleLevel
        {
            get { return m_initialInternalResistanceOnBatteryModuleLevel;  }
            set { m_initialInternalResistanceOnBatteryModuleLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "CurrentInternalResistanceOnBatteryModuleLevel", IsRequired = false, Order = 34)]
        public long CurrentInternalResistanceOnBatteryModuleLevel
        {
            get { return m_currentInternalResistanceOnBatteryModuleLevel;  }
            set { m_currentInternalResistanceOnBatteryModuleLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "InternalResistanceIncreaseOnBatteryModuleLevel", IsRequired = false, Order = 35)]
        public long InternalResistanceIncreaseOnBatteryModuleLevel
        {
            get { return m_internalResistanceIncreaseOnBatteryModuleLevel;  }
            set { m_internalResistanceIncreaseOnBatteryModuleLevel = value; }
        }

        /// <remarks />
        [DataMember(Name = "ExpectedLifetime_NumberOfCharge_DischargeCycles", IsRequired = false, Order = 36)]
        public long ExpectedLifetime_NumberOfCharge_DischargeCycles
        {
            get { return m_expectedLifetime_NumberOfCharge_DischargeCycles;  }
            set { m_expectedLifetime_NumberOfCharge_DischargeCycles = value; }
        }

        /// <remarks />
        [DataMember(Name = "NumberOf_Full_Charge_DischargeCycles", IsRequired = false, Order = 37)]
        public long NumberOf_Full_Charge_DischargeCycles
        {
            get { return m_numberOf_Full_Charge_DischargeCycles;  }
            set { m_numberOf_Full_Charge_DischargeCycles = value; }
        }

        /// <remarks />
        [DataMember(Name = "Cycle_LifeReferenceTest", IsRequired = false, Order = 38)]
        public string Cycle_LifeReferenceTest
        {
            get { return m_cycle_LifeReferenceTest;  }
            set { m_cycle_LifeReferenceTest = value; }
        }

        /// <remarks />
        [DataMember(Name = "C_RateOfRelevantCycle_LifeTest", IsRequired = false, Order = 39)]
        public double C_RateOfRelevantCycle_LifeTest
        {
            get { return m_c_RateOfRelevantCycle_LifeTest;  }
            set { m_c_RateOfRelevantCycle_LifeTest = value; }
        }

        /// <remarks />
        [DataMember(Name = "EnergyThroughput", IsRequired = false, Order = 40)]
        public double EnergyThroughput
        {
            get { return m_energyThroughput;  }
            set { m_energyThroughput = value; }
        }

        /// <remarks />
        [DataMember(Name = "CapacityThroughput", IsRequired = false, Order = 41)]
        public double CapacityThroughput
        {
            get { return m_capacityThroughput;  }
            set { m_capacityThroughput = value; }
        }

        /// <remarks />
        [DataMember(Name = "CapacityThresholdForExhaustion", IsRequired = false, Order = 42)]
        public long CapacityThresholdForExhaustion
        {
            get { return m_capacityThresholdForExhaustion;  }
            set { m_capacityThresholdForExhaustion = value; }
        }

        /// <remarks />
        [DataMember(Name = "SOCEThresholdForExhaustion", IsRequired = false, Order = 43)]
        public long SOCEThresholdForExhaustion
        {
            get { return m_sOCEThresholdForExhaustion;  }
            set { m_sOCEThresholdForExhaustion = value; }
        }

        /// <remarks />
        [DataMember(Name = "WarrantyPeriodOfTheBattery", IsRequired = false, Order = 44)]
        public long WarrantyPeriodOfTheBattery
        {
            get { return m_warrantyPeriodOfTheBattery;  }
            set { m_warrantyPeriodOfTheBattery = value; }
        }

        /// <remarks />
        [DataMember(Name = "DateOfPuttingTheBatteryIntoService", IsRequired = false, Order = 45)]
        public DateTime DateOfPuttingTheBatteryIntoService
        {
            get { return m_dateOfPuttingTheBatteryIntoService;  }
            set { m_dateOfPuttingTheBatteryIntoService = value; }
        }

        /// <remarks />
        [DataMember(Name = "TemperatureRangeIdleState_LowerBoundary", IsRequired = false, Order = 46)]
        public long TemperatureRangeIdleState_LowerBoundary
        {
            get { return m_temperatureRangeIdleState_LowerBoundary;  }
            set { m_temperatureRangeIdleState_LowerBoundary = value; }
        }

        /// <remarks />
        [DataMember(Name = "TemperatureRangeIdleState_UpperBoundary", IsRequired = false, Order = 47)]
        public long TemperatureRangeIdleState_UpperBoundary
        {
            get { return m_temperatureRangeIdleState_UpperBoundary;  }
            set { m_temperatureRangeIdleState_UpperBoundary = value; }
        }

        /// <remarks />
        [DataMember(Name = "TimeSpentInExtremeTemperaturesAboveBoundary", IsRequired = false, Order = 48)]
        public long TimeSpentInExtremeTemperaturesAboveBoundary
        {
            get { return m_timeSpentInExtremeTemperaturesAboveBoundary;  }
            set { m_timeSpentInExtremeTemperaturesAboveBoundary = value; }
        }

        /// <remarks />
        [DataMember(Name = "TimeSpentInExtremeTemperaturesBelowBoundary", IsRequired = false, Order = 49)]
        public long TimeSpentInExtremeTemperaturesBelowBoundary
        {
            get { return m_timeSpentInExtremeTemperaturesBelowBoundary;  }
            set { m_timeSpentInExtremeTemperaturesBelowBoundary = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.PerformanceAndDurabilityDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.PerformanceAndDurabilityDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.PerformanceAndDurabilityDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.PerformanceAndDurabilityDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteInt64("TimeSpentChargingDuringExtremeTemperaturesAboveBoundary", TimeSpentChargingDuringExtremeTemperaturesAboveBoundary);
            encoder.WriteInt64("TimeSpentChargingDuringExtremeTemperaturesBelowBoundary", TimeSpentChargingDuringExtremeTemperaturesBelowBoundary);
            encoder.WriteInt64("InformationOnAccidents", InformationOnAccidents);
            encoder.WriteInt64("NumberOfDeepDischargeEvents", NumberOfDeepDischargeEvents);
            encoder.WriteInt64("NumberOfOverchargeEvents", NumberOfOverchargeEvents);
            encoder.WriteInt64("CertifiedUsableBatteryEnergy_UBEcertified", CertifiedUsableBatteryEnergy_UBEcertified);
            encoder.WriteInt64("RemainingUsableBatteryEnergy_UBEmeasured", RemainingUsableBatteryEnergy_UBEmeasured);
            encoder.WriteInt64("StateOfCertifiedEnergy_SOCE", StateOfCertifiedEnergy_SOCE);
            encoder.WriteDouble("InitialSelf_DischargingRate", InitialSelf_DischargingRate);
            encoder.WriteDouble("CurrentSelf_DischargingRate", CurrentSelf_DischargingRate);
            encoder.WriteInt64("EvolutionOfSelf_DischargingRates", EvolutionOfSelf_DischargingRates);
            encoder.WriteInt64("RatedCapacity", RatedCapacity);
            encoder.WriteInt64("RemainingCapacity", RemainingCapacity);
            encoder.WriteInt64("CapacityFade", CapacityFade);
            encoder.WriteInt64("StateOfCharge_SoC", StateOfCharge_SoC);
            encoder.WriteInt64("NominalVoltage", NominalVoltage);
            encoder.WriteInt64("MinimumVoltage", MinimumVoltage);
            encoder.WriteInt64("MaximumVoltage", MaximumVoltage);
            encoder.WriteInt64("OriginalPowerCapability", OriginalPowerCapability);
            encoder.WriteInt64("RemainingPowerCapability", RemainingPowerCapability);
            encoder.WriteInt64("PowerCapabilityFade", PowerCapabilityFade);
            encoder.WriteInt64("MaximumPermittedBatteryPower", MaximumPermittedBatteryPower);
            encoder.WriteInt64("RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh", RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh);
            encoder.WriteInt64("InitialRoundTripEnergyEfficiency", InitialRoundTripEnergyEfficiency);
            encoder.WriteInt64("RoundTripEnergyEfficiencyAt50_OfCycleLife", RoundTripEnergyEfficiencyAt50_OfCycleLife);
            encoder.WriteInt64("RemainingRoundTripEnergyEfficiency", RemainingRoundTripEnergyEfficiency);
            encoder.WriteInt64("RoundTripEnergyEfficiencyFade", RoundTripEnergyEfficiencyFade);
            encoder.WriteInt64("InitialInternalResistanceOnBatteryCellLevel", InitialInternalResistanceOnBatteryCellLevel);
            encoder.WriteInt64("CurrentInternalResistanceOnBatteryCellLevel", CurrentInternalResistanceOnBatteryCellLevel);
            encoder.WriteInt64("InternalResistanceIncreaseOnBatteryCellLevel", InternalResistanceIncreaseOnBatteryCellLevel);
            encoder.WriteInt64("InitialInternalResistanceOnBatteryPackLevel", InitialInternalResistanceOnBatteryPackLevel);
            encoder.WriteInt64("CurrentInternalResistanceOnBatteryPackLevel", CurrentInternalResistanceOnBatteryPackLevel);
            encoder.WriteInt64("InitialInternalResistanceOnBatteryModuleLevel", InitialInternalResistanceOnBatteryModuleLevel);
            encoder.WriteInt64("CurrentInternalResistanceOnBatteryModuleLevel", CurrentInternalResistanceOnBatteryModuleLevel);
            encoder.WriteInt64("InternalResistanceIncreaseOnBatteryModuleLevel", InternalResistanceIncreaseOnBatteryModuleLevel);
            encoder.WriteInt64("ExpectedLifetime_NumberOfCharge_DischargeCycles", ExpectedLifetime_NumberOfCharge_DischargeCycles);
            encoder.WriteInt64("NumberOf_Full_Charge_DischargeCycles", NumberOf_Full_Charge_DischargeCycles);
            encoder.WriteString("Cycle_LifeReferenceTest", Cycle_LifeReferenceTest);
            encoder.WriteDouble("C_RateOfRelevantCycle_LifeTest", C_RateOfRelevantCycle_LifeTest);
            encoder.WriteDouble("EnergyThroughput", EnergyThroughput);
            encoder.WriteDouble("CapacityThroughput", CapacityThroughput);
            encoder.WriteInt64("CapacityThresholdForExhaustion", CapacityThresholdForExhaustion);
            encoder.WriteInt64("SOCEThresholdForExhaustion", SOCEThresholdForExhaustion);
            encoder.WriteInt64("WarrantyPeriodOfTheBattery", WarrantyPeriodOfTheBattery);
            encoder.WriteDateTime("DateOfPuttingTheBatteryIntoService", DateOfPuttingTheBatteryIntoService);
            encoder.WriteInt64("TemperatureRangeIdleState_LowerBoundary", TemperatureRangeIdleState_LowerBoundary);
            encoder.WriteInt64("TemperatureRangeIdleState_UpperBoundary", TemperatureRangeIdleState_UpperBoundary);
            encoder.WriteInt64("TimeSpentInExtremeTemperaturesAboveBoundary", TimeSpentInExtremeTemperaturesAboveBoundary);
            encoder.WriteInt64("TimeSpentInExtremeTemperaturesBelowBoundary", TimeSpentInExtremeTemperaturesBelowBoundary);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            TimeSpentChargingDuringExtremeTemperaturesAboveBoundary = decoder.ReadInt64("TimeSpentChargingDuringExtremeTemperaturesAboveBoundary");
            TimeSpentChargingDuringExtremeTemperaturesBelowBoundary = decoder.ReadInt64("TimeSpentChargingDuringExtremeTemperaturesBelowBoundary");
            InformationOnAccidents = decoder.ReadInt64("InformationOnAccidents");
            NumberOfDeepDischargeEvents = decoder.ReadInt64("NumberOfDeepDischargeEvents");
            NumberOfOverchargeEvents = decoder.ReadInt64("NumberOfOverchargeEvents");
            CertifiedUsableBatteryEnergy_UBEcertified = decoder.ReadInt64("CertifiedUsableBatteryEnergy_UBEcertified");
            RemainingUsableBatteryEnergy_UBEmeasured = decoder.ReadInt64("RemainingUsableBatteryEnergy_UBEmeasured");
            StateOfCertifiedEnergy_SOCE = decoder.ReadInt64("StateOfCertifiedEnergy_SOCE");
            InitialSelf_DischargingRate = decoder.ReadDouble("InitialSelf_DischargingRate");
            CurrentSelf_DischargingRate = decoder.ReadDouble("CurrentSelf_DischargingRate");
            EvolutionOfSelf_DischargingRates = decoder.ReadInt64("EvolutionOfSelf_DischargingRates");
            RatedCapacity = decoder.ReadInt64("RatedCapacity");
            RemainingCapacity = decoder.ReadInt64("RemainingCapacity");
            CapacityFade = decoder.ReadInt64("CapacityFade");
            StateOfCharge_SoC = decoder.ReadInt64("StateOfCharge_SoC");
            NominalVoltage = decoder.ReadInt64("NominalVoltage");
            MinimumVoltage = decoder.ReadInt64("MinimumVoltage");
            MaximumVoltage = decoder.ReadInt64("MaximumVoltage");
            OriginalPowerCapability = decoder.ReadInt64("OriginalPowerCapability");
            RemainingPowerCapability = decoder.ReadInt64("RemainingPowerCapability");
            PowerCapabilityFade = decoder.ReadInt64("PowerCapabilityFade");
            MaximumPermittedBatteryPower = decoder.ReadInt64("MaximumPermittedBatteryPower");
            RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh = decoder.ReadInt64("RatioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh");
            InitialRoundTripEnergyEfficiency = decoder.ReadInt64("InitialRoundTripEnergyEfficiency");
            RoundTripEnergyEfficiencyAt50_OfCycleLife = decoder.ReadInt64("RoundTripEnergyEfficiencyAt50_OfCycleLife");
            RemainingRoundTripEnergyEfficiency = decoder.ReadInt64("RemainingRoundTripEnergyEfficiency");
            RoundTripEnergyEfficiencyFade = decoder.ReadInt64("RoundTripEnergyEfficiencyFade");
            InitialInternalResistanceOnBatteryCellLevel = decoder.ReadInt64("InitialInternalResistanceOnBatteryCellLevel");
            CurrentInternalResistanceOnBatteryCellLevel = decoder.ReadInt64("CurrentInternalResistanceOnBatteryCellLevel");
            InternalResistanceIncreaseOnBatteryCellLevel = decoder.ReadInt64("InternalResistanceIncreaseOnBatteryCellLevel");
            InitialInternalResistanceOnBatteryPackLevel = decoder.ReadInt64("InitialInternalResistanceOnBatteryPackLevel");
            CurrentInternalResistanceOnBatteryPackLevel = decoder.ReadInt64("CurrentInternalResistanceOnBatteryPackLevel");
            InitialInternalResistanceOnBatteryModuleLevel = decoder.ReadInt64("InitialInternalResistanceOnBatteryModuleLevel");
            CurrentInternalResistanceOnBatteryModuleLevel = decoder.ReadInt64("CurrentInternalResistanceOnBatteryModuleLevel");
            InternalResistanceIncreaseOnBatteryModuleLevel = decoder.ReadInt64("InternalResistanceIncreaseOnBatteryModuleLevel");
            ExpectedLifetime_NumberOfCharge_DischargeCycles = decoder.ReadInt64("ExpectedLifetime_NumberOfCharge_DischargeCycles");
            NumberOf_Full_Charge_DischargeCycles = decoder.ReadInt64("NumberOf_Full_Charge_DischargeCycles");
            Cycle_LifeReferenceTest = decoder.ReadString("Cycle_LifeReferenceTest");
            C_RateOfRelevantCycle_LifeTest = decoder.ReadDouble("C_RateOfRelevantCycle_LifeTest");
            EnergyThroughput = decoder.ReadDouble("EnergyThroughput");
            CapacityThroughput = decoder.ReadDouble("CapacityThroughput");
            CapacityThresholdForExhaustion = decoder.ReadInt64("CapacityThresholdForExhaustion");
            SOCEThresholdForExhaustion = decoder.ReadInt64("SOCEThresholdForExhaustion");
            WarrantyPeriodOfTheBattery = decoder.ReadInt64("WarrantyPeriodOfTheBattery");
            DateOfPuttingTheBatteryIntoService = decoder.ReadDateTime("DateOfPuttingTheBatteryIntoService");
            TemperatureRangeIdleState_LowerBoundary = decoder.ReadInt64("TemperatureRangeIdleState_LowerBoundary");
            TemperatureRangeIdleState_UpperBoundary = decoder.ReadInt64("TemperatureRangeIdleState_UpperBoundary");
            TimeSpentInExtremeTemperaturesAboveBoundary = decoder.ReadInt64("TimeSpentInExtremeTemperaturesAboveBoundary");
            TimeSpentInExtremeTemperaturesBelowBoundary = decoder.ReadInt64("TimeSpentInExtremeTemperaturesBelowBoundary");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            PerformanceAndDurabilityDataType value = encodeable as PerformanceAndDurabilityDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary, value.m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary)) return false;
            if (!Utils.IsEqual(m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary, value.m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary)) return false;
            if (!Utils.IsEqual(m_informationOnAccidents, value.m_informationOnAccidents)) return false;
            if (!Utils.IsEqual(m_numberOfDeepDischargeEvents, value.m_numberOfDeepDischargeEvents)) return false;
            if (!Utils.IsEqual(m_numberOfOverchargeEvents, value.m_numberOfOverchargeEvents)) return false;
            if (!Utils.IsEqual(m_certifiedUsableBatteryEnergy_UBEcertified, value.m_certifiedUsableBatteryEnergy_UBEcertified)) return false;
            if (!Utils.IsEqual(m_remainingUsableBatteryEnergy_UBEmeasured, value.m_remainingUsableBatteryEnergy_UBEmeasured)) return false;
            if (!Utils.IsEqual(m_stateOfCertifiedEnergy_SOCE, value.m_stateOfCertifiedEnergy_SOCE)) return false;
            if (!Utils.IsEqual(m_initialSelf_DischargingRate, value.m_initialSelf_DischargingRate)) return false;
            if (!Utils.IsEqual(m_currentSelf_DischargingRate, value.m_currentSelf_DischargingRate)) return false;
            if (!Utils.IsEqual(m_evolutionOfSelf_DischargingRates, value.m_evolutionOfSelf_DischargingRates)) return false;
            if (!Utils.IsEqual(m_ratedCapacity, value.m_ratedCapacity)) return false;
            if (!Utils.IsEqual(m_remainingCapacity, value.m_remainingCapacity)) return false;
            if (!Utils.IsEqual(m_capacityFade, value.m_capacityFade)) return false;
            if (!Utils.IsEqual(m_stateOfCharge_SoC, value.m_stateOfCharge_SoC)) return false;
            if (!Utils.IsEqual(m_nominalVoltage, value.m_nominalVoltage)) return false;
            if (!Utils.IsEqual(m_minimumVoltage, value.m_minimumVoltage)) return false;
            if (!Utils.IsEqual(m_maximumVoltage, value.m_maximumVoltage)) return false;
            if (!Utils.IsEqual(m_originalPowerCapability, value.m_originalPowerCapability)) return false;
            if (!Utils.IsEqual(m_remainingPowerCapability, value.m_remainingPowerCapability)) return false;
            if (!Utils.IsEqual(m_powerCapabilityFade, value.m_powerCapabilityFade)) return false;
            if (!Utils.IsEqual(m_maximumPermittedBatteryPower, value.m_maximumPermittedBatteryPower)) return false;
            if (!Utils.IsEqual(m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh, value.m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh)) return false;
            if (!Utils.IsEqual(m_initialRoundTripEnergyEfficiency, value.m_initialRoundTripEnergyEfficiency)) return false;
            if (!Utils.IsEqual(m_roundTripEnergyEfficiencyAt50_OfCycleLife, value.m_roundTripEnergyEfficiencyAt50_OfCycleLife)) return false;
            if (!Utils.IsEqual(m_remainingRoundTripEnergyEfficiency, value.m_remainingRoundTripEnergyEfficiency)) return false;
            if (!Utils.IsEqual(m_roundTripEnergyEfficiencyFade, value.m_roundTripEnergyEfficiencyFade)) return false;
            if (!Utils.IsEqual(m_initialInternalResistanceOnBatteryCellLevel, value.m_initialInternalResistanceOnBatteryCellLevel)) return false;
            if (!Utils.IsEqual(m_currentInternalResistanceOnBatteryCellLevel, value.m_currentInternalResistanceOnBatteryCellLevel)) return false;
            if (!Utils.IsEqual(m_internalResistanceIncreaseOnBatteryCellLevel, value.m_internalResistanceIncreaseOnBatteryCellLevel)) return false;
            if (!Utils.IsEqual(m_initialInternalResistanceOnBatteryPackLevel, value.m_initialInternalResistanceOnBatteryPackLevel)) return false;
            if (!Utils.IsEqual(m_currentInternalResistanceOnBatteryPackLevel, value.m_currentInternalResistanceOnBatteryPackLevel)) return false;
            if (!Utils.IsEqual(m_initialInternalResistanceOnBatteryModuleLevel, value.m_initialInternalResistanceOnBatteryModuleLevel)) return false;
            if (!Utils.IsEqual(m_currentInternalResistanceOnBatteryModuleLevel, value.m_currentInternalResistanceOnBatteryModuleLevel)) return false;
            if (!Utils.IsEqual(m_internalResistanceIncreaseOnBatteryModuleLevel, value.m_internalResistanceIncreaseOnBatteryModuleLevel)) return false;
            if (!Utils.IsEqual(m_expectedLifetime_NumberOfCharge_DischargeCycles, value.m_expectedLifetime_NumberOfCharge_DischargeCycles)) return false;
            if (!Utils.IsEqual(m_numberOf_Full_Charge_DischargeCycles, value.m_numberOf_Full_Charge_DischargeCycles)) return false;
            if (!Utils.IsEqual(m_cycle_LifeReferenceTest, value.m_cycle_LifeReferenceTest)) return false;
            if (!Utils.IsEqual(m_c_RateOfRelevantCycle_LifeTest, value.m_c_RateOfRelevantCycle_LifeTest)) return false;
            if (!Utils.IsEqual(m_energyThroughput, value.m_energyThroughput)) return false;
            if (!Utils.IsEqual(m_capacityThroughput, value.m_capacityThroughput)) return false;
            if (!Utils.IsEqual(m_capacityThresholdForExhaustion, value.m_capacityThresholdForExhaustion)) return false;
            if (!Utils.IsEqual(m_sOCEThresholdForExhaustion, value.m_sOCEThresholdForExhaustion)) return false;
            if (!Utils.IsEqual(m_warrantyPeriodOfTheBattery, value.m_warrantyPeriodOfTheBattery)) return false;
            if (!Utils.IsEqual(m_dateOfPuttingTheBatteryIntoService, value.m_dateOfPuttingTheBatteryIntoService)) return false;
            if (!Utils.IsEqual(m_temperatureRangeIdleState_LowerBoundary, value.m_temperatureRangeIdleState_LowerBoundary)) return false;
            if (!Utils.IsEqual(m_temperatureRangeIdleState_UpperBoundary, value.m_temperatureRangeIdleState_UpperBoundary)) return false;
            if (!Utils.IsEqual(m_timeSpentInExtremeTemperaturesAboveBoundary, value.m_timeSpentInExtremeTemperaturesAboveBoundary)) return false;
            if (!Utils.IsEqual(m_timeSpentInExtremeTemperaturesBelowBoundary, value.m_timeSpentInExtremeTemperaturesBelowBoundary)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (PerformanceAndDurabilityDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            PerformanceAndDurabilityDataType clone = (PerformanceAndDurabilityDataType)base.MemberwiseClone();

            clone.m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary = (long)Utils.Clone(this.m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary);
            clone.m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary = (long)Utils.Clone(this.m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary);
            clone.m_informationOnAccidents = (long)Utils.Clone(this.m_informationOnAccidents);
            clone.m_numberOfDeepDischargeEvents = (long)Utils.Clone(this.m_numberOfDeepDischargeEvents);
            clone.m_numberOfOverchargeEvents = (long)Utils.Clone(this.m_numberOfOverchargeEvents);
            clone.m_certifiedUsableBatteryEnergy_UBEcertified = (long)Utils.Clone(this.m_certifiedUsableBatteryEnergy_UBEcertified);
            clone.m_remainingUsableBatteryEnergy_UBEmeasured = (long)Utils.Clone(this.m_remainingUsableBatteryEnergy_UBEmeasured);
            clone.m_stateOfCertifiedEnergy_SOCE = (long)Utils.Clone(this.m_stateOfCertifiedEnergy_SOCE);
            clone.m_initialSelf_DischargingRate = (double)Utils.Clone(this.m_initialSelf_DischargingRate);
            clone.m_currentSelf_DischargingRate = (double)Utils.Clone(this.m_currentSelf_DischargingRate);
            clone.m_evolutionOfSelf_DischargingRates = (long)Utils.Clone(this.m_evolutionOfSelf_DischargingRates);
            clone.m_ratedCapacity = (long)Utils.Clone(this.m_ratedCapacity);
            clone.m_remainingCapacity = (long)Utils.Clone(this.m_remainingCapacity);
            clone.m_capacityFade = (long)Utils.Clone(this.m_capacityFade);
            clone.m_stateOfCharge_SoC = (long)Utils.Clone(this.m_stateOfCharge_SoC);
            clone.m_nominalVoltage = (long)Utils.Clone(this.m_nominalVoltage);
            clone.m_minimumVoltage = (long)Utils.Clone(this.m_minimumVoltage);
            clone.m_maximumVoltage = (long)Utils.Clone(this.m_maximumVoltage);
            clone.m_originalPowerCapability = (long)Utils.Clone(this.m_originalPowerCapability);
            clone.m_remainingPowerCapability = (long)Utils.Clone(this.m_remainingPowerCapability);
            clone.m_powerCapabilityFade = (long)Utils.Clone(this.m_powerCapabilityFade);
            clone.m_maximumPermittedBatteryPower = (long)Utils.Clone(this.m_maximumPermittedBatteryPower);
            clone.m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh = (long)Utils.Clone(this.m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh);
            clone.m_initialRoundTripEnergyEfficiency = (long)Utils.Clone(this.m_initialRoundTripEnergyEfficiency);
            clone.m_roundTripEnergyEfficiencyAt50_OfCycleLife = (long)Utils.Clone(this.m_roundTripEnergyEfficiencyAt50_OfCycleLife);
            clone.m_remainingRoundTripEnergyEfficiency = (long)Utils.Clone(this.m_remainingRoundTripEnergyEfficiency);
            clone.m_roundTripEnergyEfficiencyFade = (long)Utils.Clone(this.m_roundTripEnergyEfficiencyFade);
            clone.m_initialInternalResistanceOnBatteryCellLevel = (long)Utils.Clone(this.m_initialInternalResistanceOnBatteryCellLevel);
            clone.m_currentInternalResistanceOnBatteryCellLevel = (long)Utils.Clone(this.m_currentInternalResistanceOnBatteryCellLevel);
            clone.m_internalResistanceIncreaseOnBatteryCellLevel = (long)Utils.Clone(this.m_internalResistanceIncreaseOnBatteryCellLevel);
            clone.m_initialInternalResistanceOnBatteryPackLevel = (long)Utils.Clone(this.m_initialInternalResistanceOnBatteryPackLevel);
            clone.m_currentInternalResistanceOnBatteryPackLevel = (long)Utils.Clone(this.m_currentInternalResistanceOnBatteryPackLevel);
            clone.m_initialInternalResistanceOnBatteryModuleLevel = (long)Utils.Clone(this.m_initialInternalResistanceOnBatteryModuleLevel);
            clone.m_currentInternalResistanceOnBatteryModuleLevel = (long)Utils.Clone(this.m_currentInternalResistanceOnBatteryModuleLevel);
            clone.m_internalResistanceIncreaseOnBatteryModuleLevel = (long)Utils.Clone(this.m_internalResistanceIncreaseOnBatteryModuleLevel);
            clone.m_expectedLifetime_NumberOfCharge_DischargeCycles = (long)Utils.Clone(this.m_expectedLifetime_NumberOfCharge_DischargeCycles);
            clone.m_numberOf_Full_Charge_DischargeCycles = (long)Utils.Clone(this.m_numberOf_Full_Charge_DischargeCycles);
            clone.m_cycle_LifeReferenceTest = (string)Utils.Clone(this.m_cycle_LifeReferenceTest);
            clone.m_c_RateOfRelevantCycle_LifeTest = (double)Utils.Clone(this.m_c_RateOfRelevantCycle_LifeTest);
            clone.m_energyThroughput = (double)Utils.Clone(this.m_energyThroughput);
            clone.m_capacityThroughput = (double)Utils.Clone(this.m_capacityThroughput);
            clone.m_capacityThresholdForExhaustion = (long)Utils.Clone(this.m_capacityThresholdForExhaustion);
            clone.m_sOCEThresholdForExhaustion = (long)Utils.Clone(this.m_sOCEThresholdForExhaustion);
            clone.m_warrantyPeriodOfTheBattery = (long)Utils.Clone(this.m_warrantyPeriodOfTheBattery);
            clone.m_dateOfPuttingTheBatteryIntoService = (DateTime)Utils.Clone(this.m_dateOfPuttingTheBatteryIntoService);
            clone.m_temperatureRangeIdleState_LowerBoundary = (long)Utils.Clone(this.m_temperatureRangeIdleState_LowerBoundary);
            clone.m_temperatureRangeIdleState_UpperBoundary = (long)Utils.Clone(this.m_temperatureRangeIdleState_UpperBoundary);
            clone.m_timeSpentInExtremeTemperaturesAboveBoundary = (long)Utils.Clone(this.m_timeSpentInExtremeTemperaturesAboveBoundary);
            clone.m_timeSpentInExtremeTemperaturesBelowBoundary = (long)Utils.Clone(this.m_timeSpentInExtremeTemperaturesBelowBoundary);

            return clone;
        }
        #endregion

        #region Private Fields
        private long m_timeSpentChargingDuringExtremeTemperaturesAboveBoundary;
        private long m_timeSpentChargingDuringExtremeTemperaturesBelowBoundary;
        private long m_informationOnAccidents;
        private long m_numberOfDeepDischargeEvents;
        private long m_numberOfOverchargeEvents;
        private long m_certifiedUsableBatteryEnergy_UBEcertified;
        private long m_remainingUsableBatteryEnergy_UBEmeasured;
        private long m_stateOfCertifiedEnergy_SOCE;
        private double m_initialSelf_DischargingRate;
        private double m_currentSelf_DischargingRate;
        private long m_evolutionOfSelf_DischargingRates;
        private long m_ratedCapacity;
        private long m_remainingCapacity;
        private long m_capacityFade;
        private long m_stateOfCharge_SoC;
        private long m_nominalVoltage;
        private long m_minimumVoltage;
        private long m_maximumVoltage;
        private long m_originalPowerCapability;
        private long m_remainingPowerCapability;
        private long m_powerCapabilityFade;
        private long m_maximumPermittedBatteryPower;
        private long m_ratioBetweenNominalAllowedBatteryPower_W_AndBatteryEnergy_Wh;
        private long m_initialRoundTripEnergyEfficiency;
        private long m_roundTripEnergyEfficiencyAt50_OfCycleLife;
        private long m_remainingRoundTripEnergyEfficiency;
        private long m_roundTripEnergyEfficiencyFade;
        private long m_initialInternalResistanceOnBatteryCellLevel;
        private long m_currentInternalResistanceOnBatteryCellLevel;
        private long m_internalResistanceIncreaseOnBatteryCellLevel;
        private long m_initialInternalResistanceOnBatteryPackLevel;
        private long m_currentInternalResistanceOnBatteryPackLevel;
        private long m_initialInternalResistanceOnBatteryModuleLevel;
        private long m_currentInternalResistanceOnBatteryModuleLevel;
        private long m_internalResistanceIncreaseOnBatteryModuleLevel;
        private long m_expectedLifetime_NumberOfCharge_DischargeCycles;
        private long m_numberOf_Full_Charge_DischargeCycles;
        private string m_cycle_LifeReferenceTest;
        private double m_c_RateOfRelevantCycle_LifeTest;
        private double m_energyThroughput;
        private double m_capacityThroughput;
        private long m_capacityThresholdForExhaustion;
        private long m_sOCEThresholdForExhaustion;
        private long m_warrantyPeriodOfTheBattery;
        private DateTime m_dateOfPuttingTheBatteryIntoService;
        private long m_temperatureRangeIdleState_LowerBoundary;
        private long m_temperatureRangeIdleState_UpperBoundary;
        private long m_timeSpentInExtremeTemperaturesAboveBoundary;
        private long m_timeSpentInExtremeTemperaturesBelowBoundary;
        #endregion
    }

    #region PerformanceAndDurabilityDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfPerformanceAndDurabilityDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "PerformanceAndDurabilityDataType")]
    public partial class PerformanceAndDurabilityDataTypeCollection : List<PerformanceAndDurabilityDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public PerformanceAndDurabilityDataTypeCollection() {}

        /// <remarks />
        public PerformanceAndDurabilityDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public PerformanceAndDurabilityDataTypeCollection(IEnumerable<PerformanceAndDurabilityDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator PerformanceAndDurabilityDataTypeCollection(PerformanceAndDurabilityDataType[] values)
        {
            if (values != null)
            {
                return new PerformanceAndDurabilityDataTypeCollection(values);
            }

            return new PerformanceAndDurabilityDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator PerformanceAndDurabilityDataType[](PerformanceAndDurabilityDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (PerformanceAndDurabilityDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            PerformanceAndDurabilityDataTypeCollection clone = new PerformanceAndDurabilityDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((PerformanceAndDurabilityDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region SupplyChainDueDiligenceDataType Class
    #if (!OPCUA_EXCLUDE_SupplyChainDueDiligenceDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = BatteryPassport.Namespaces.BatteryPassport)]
    public partial class SupplyChainDueDiligenceDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public SupplyChainDueDiligenceDataType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_informationOfTheDueDiligenceReport = null;
            m_thirdPartySupplyChainAssurances = null;
            m_eUTaxonomyDisclosureStatement = null;
            m_sustainabilityReport = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "InformationOfTheDueDiligenceReport", IsRequired = false, Order = 1)]
        public string InformationOfTheDueDiligenceReport
        {
            get { return m_informationOfTheDueDiligenceReport;  }
            set { m_informationOfTheDueDiligenceReport = value; }
        }

        /// <remarks />
        [DataMember(Name = "ThirdPartySupplyChainAssurances", IsRequired = false, Order = 2)]
        public string ThirdPartySupplyChainAssurances
        {
            get { return m_thirdPartySupplyChainAssurances;  }
            set { m_thirdPartySupplyChainAssurances = value; }
        }

        /// <remarks />
        [DataMember(Name = "EUTaxonomyDisclosureStatement", IsRequired = false, Order = 3)]
        public string EUTaxonomyDisclosureStatement
        {
            get { return m_eUTaxonomyDisclosureStatement;  }
            set { m_eUTaxonomyDisclosureStatement = value; }
        }

        /// <remarks />
        [DataMember(Name = "SustainabilityReport", IsRequired = false, Order = 4)]
        public string SustainabilityReport
        {
            get { return m_sustainabilityReport;  }
            set { m_sustainabilityReport = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.SupplyChainDueDiligenceDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.SupplyChainDueDiligenceDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.SupplyChainDueDiligenceDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.SupplyChainDueDiligenceDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            encoder.WriteString("InformationOfTheDueDiligenceReport", InformationOfTheDueDiligenceReport);
            encoder.WriteString("ThirdPartySupplyChainAssurances", ThirdPartySupplyChainAssurances);
            encoder.WriteString("EUTaxonomyDisclosureStatement", EUTaxonomyDisclosureStatement);
            encoder.WriteString("SustainabilityReport", SustainabilityReport);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(BatteryPassport.Namespaces.BatteryPassport);

            InformationOfTheDueDiligenceReport = decoder.ReadString("InformationOfTheDueDiligenceReport");
            ThirdPartySupplyChainAssurances = decoder.ReadString("ThirdPartySupplyChainAssurances");
            EUTaxonomyDisclosureStatement = decoder.ReadString("EUTaxonomyDisclosureStatement");
            SustainabilityReport = decoder.ReadString("SustainabilityReport");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            SupplyChainDueDiligenceDataType value = encodeable as SupplyChainDueDiligenceDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_informationOfTheDueDiligenceReport, value.m_informationOfTheDueDiligenceReport)) return false;
            if (!Utils.IsEqual(m_thirdPartySupplyChainAssurances, value.m_thirdPartySupplyChainAssurances)) return false;
            if (!Utils.IsEqual(m_eUTaxonomyDisclosureStatement, value.m_eUTaxonomyDisclosureStatement)) return false;
            if (!Utils.IsEqual(m_sustainabilityReport, value.m_sustainabilityReport)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (SupplyChainDueDiligenceDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            SupplyChainDueDiligenceDataType clone = (SupplyChainDueDiligenceDataType)base.MemberwiseClone();

            clone.m_informationOfTheDueDiligenceReport = (string)Utils.Clone(this.m_informationOfTheDueDiligenceReport);
            clone.m_thirdPartySupplyChainAssurances = (string)Utils.Clone(this.m_thirdPartySupplyChainAssurances);
            clone.m_eUTaxonomyDisclosureStatement = (string)Utils.Clone(this.m_eUTaxonomyDisclosureStatement);
            clone.m_sustainabilityReport = (string)Utils.Clone(this.m_sustainabilityReport);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_informationOfTheDueDiligenceReport;
        private string m_thirdPartySupplyChainAssurances;
        private string m_eUTaxonomyDisclosureStatement;
        private string m_sustainabilityReport;
        #endregion
    }

    #region SupplyChainDueDiligenceDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfSupplyChainDueDiligenceDataType", Namespace = BatteryPassport.Namespaces.BatteryPassport, ItemName = "SupplyChainDueDiligenceDataType")]
    public partial class SupplyChainDueDiligenceDataTypeCollection : List<SupplyChainDueDiligenceDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public SupplyChainDueDiligenceDataTypeCollection() {}

        /// <remarks />
        public SupplyChainDueDiligenceDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public SupplyChainDueDiligenceDataTypeCollection(IEnumerable<SupplyChainDueDiligenceDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator SupplyChainDueDiligenceDataTypeCollection(SupplyChainDueDiligenceDataType[] values)
        {
            if (values != null)
            {
                return new SupplyChainDueDiligenceDataTypeCollection(values);
            }

            return new SupplyChainDueDiligenceDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator SupplyChainDueDiligenceDataType[](SupplyChainDueDiligenceDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (SupplyChainDueDiligenceDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            SupplyChainDueDiligenceDataTypeCollection clone = new SupplyChainDueDiligenceDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((SupplyChainDueDiligenceDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion
}