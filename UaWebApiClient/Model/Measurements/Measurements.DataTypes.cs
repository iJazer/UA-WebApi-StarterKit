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

namespace Measurements
{
    #region OrientationDataType Class
    #if (!OPCUA_EXCLUDE_OrientationDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = Measurements.Namespaces.opcfoundation.org:2024_10:starterkit:measurements)]
    public partial class OrientationDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public OrientationDataType()
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
            m_profileName = null;
            m_x = (double)0;
            m_y = (double)0;
            m_rotation = (double)0;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "ProfileName", IsRequired = false, Order = 1)]
        public string ProfileName
        {
            get { return m_profileName;  }
            set { m_profileName = value; }
        }

        /// <remarks />
        [DataMember(Name = "X", IsRequired = false, Order = 2)]
        public double X
        {
            get { return m_x;  }
            set { m_x = value; }
        }

        /// <remarks />
        [DataMember(Name = "Y", IsRequired = false, Order = 3)]
        public double Y
        {
            get { return m_y;  }
            set { m_y = value; }
        }

        /// <remarks />
        [DataMember(Name = "Rotation", IsRequired = false, Order = 4)]
        public double Rotation
        {
            get { return m_rotation;  }
            set { m_rotation = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.OrientationDataType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.OrientationDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.OrientationDataType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.OrientationDataType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Measurements.Namespaces.opcfoundation.org:2024_10:starterkit:measurements);

            encoder.WriteString("ProfileName", ProfileName);
            encoder.WriteDouble("X", X);
            encoder.WriteDouble("Y", Y);
            encoder.WriteDouble("Rotation", Rotation);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Measurements.Namespaces.opcfoundation.org:2024_10:starterkit:measurements);

            ProfileName = decoder.ReadString("ProfileName");
            X = decoder.ReadDouble("X");
            Y = decoder.ReadDouble("Y");
            Rotation = decoder.ReadDouble("Rotation");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            OrientationDataType value = encodeable as OrientationDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_profileName, value.m_profileName)) return false;
            if (!Utils.IsEqual(m_x, value.m_x)) return false;
            if (!Utils.IsEqual(m_y, value.m_y)) return false;
            if (!Utils.IsEqual(m_rotation, value.m_rotation)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (OrientationDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            OrientationDataType clone = (OrientationDataType)base.MemberwiseClone();

            clone.m_profileName = (string)Utils.Clone(this.m_profileName);
            clone.m_x = (double)Utils.Clone(this.m_x);
            clone.m_y = (double)Utils.Clone(this.m_y);
            clone.m_rotation = (double)Utils.Clone(this.m_rotation);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_profileName;
        private double m_x;
        private double m_y;
        private double m_rotation;
        #endregion
    }

    #region OrientationDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfOrientationDataType", Namespace = Measurements.Namespaces.opcfoundation.org:2024_10:starterkit:measurements, ItemName = "OrientationDataType")]
    public partial class OrientationDataTypeCollection : List<OrientationDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public OrientationDataTypeCollection() {}

        /// <remarks />
        public OrientationDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public OrientationDataTypeCollection(IEnumerable<OrientationDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator OrientationDataTypeCollection(OrientationDataType[] values)
        {
            if (values != null)
            {
                return new OrientationDataTypeCollection(values);
            }

            return new OrientationDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator OrientationDataType[](OrientationDataTypeCollection values)
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
            return (OrientationDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            OrientationDataTypeCollection clone = new OrientationDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((OrientationDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion
}