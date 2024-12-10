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
    #region MeasurementContainerState Class
    #if (!OPCUA_EXCLUDE_MeasurementContainerState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class MeasurementContainerState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public MeasurementContainerState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Measurements.ObjectTypes.MeasurementContainerType, Measurements.Namespaces.Measurements, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADUAAAB1cm46b3BjZm91bmRhdGlvbi5vcmc6MjAyNC0xMDpzdGFydGVya2l0Om1lYXN1cmVtZW50" +
           "c/////8EYIACAQAAAAEAIAAAAE1lYXN1cmVtZW50Q29udGFpbmVyVHlwZUluc3RhbmNlAQEBAAEBAQAB" +
           "AAAA/////wQAAAAVYIkKAgAAAAEACwAAAFRlbXBlcmF0dXJlAQECAAAvAQBZRAIAAAAAC/////8BAf//" +
           "//8BAAAAFWCpCgIAAAAAABAAAABFbmdpbmVlcmluZ1VuaXRzAQEHAAAuAQBZRAcAAAAWAQB5AwFXAAAA" +
           "LwAAAGh0dHA6Ly93d3cub3BjZm91bmRhdGlvbi5vcmcvVUEvdW5pdHMvdW4vY2VmYWN0TEVDAAMCAAAA" +
           "ZW4DAAAAwrBDAwIAAABlbgcAAABDZWxzaXVzAQB3A/////8BAf////8BAAAAFWCJCgIAAAAAABAAAABF" +
           "bmdpbmVlcmluZ1VuaXRzAQEMAAAuAEQMAAAAAQB3A/////8BAf////8AAAAAFWCJCgIAAAABAAgAAABQ" +
           "cmVzc3VyZQEBDQAALwEAWUQNAAAAAAv/////AQH/////AQAAABVgqQoCAAAAAAAQAAAARW5naW5lZXJp" +
           "bmdVbml0cwEBEgAALgEAWUQSAAAAFgEAeQMBWgAAAC8AAABodHRwOi8vd3d3Lm9wY2ZvdW5kYXRpb24u" +
           "b3JnL1VBL3VuaXRzL3VuL2NlZmFjdEFQSwADAgAAAGVuAwAAAGtQYQMCAAAAZW4KAAAAa2lsb3Bhc2Nh" +
           "bAEAdwP/////AQH/////AQAAABVgiQoCAAAAAAAQAAAARW5naW5lZXJpbmdVbml0cwEBFwAALgBEFwAA" +
           "AAEAdwP/////AQH/////AAAAAARhggoEAAAAAQAFAAAAUmVzZXQBARgAAC8BARgAGAAAAAEB/////wIA" +
           "AAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQEZAAAuAEQZAAAAlgIAAAABACoBAR0AAAAOAAAA" +
           "TmV3VGVtcGVyYXR1cmUAC/////8AAAAAAAEAKgEBGgAAAAsAAABOZXdQcmVzc3VyZQAL/////wAAAAAA" +
           "AQAoAQEAAAABAAAAAgAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3VtZW50cwEBGgAA" +
           "LgBEGgAAAJYCAAAAAQAqAQEdAAAADgAAAE9sZFRlbXBlcmF0dXJlAAv/////AAAAAAABACoBARoAAAAL" +
           "AAAAT2xkUHJlc3N1cmUAC/////8AAAAAAAEAKAEBAAAAAQAAAAIAAAABAf////8AAAAAFWCJCgIAAAAB" +
           "AAsAAABPcmllbnRhdGlvbgEBGwAALwA/GwAAAAEBHwD/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public AnalogUnitState<double> Temperature
        {
            get
            {
                return m_temperature;
            }

            set
            {
                if (!Object.ReferenceEquals(m_temperature, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_temperature = value;
            }
        }

        /// <remarks />
        public AnalogUnitState<double> Pressure
        {
            get
            {
                return m_pressure;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pressure, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pressure = value;
            }
        }

        /// <remarks />
        public MeasurementResetMethodState Reset
        {
            get
            {
                return m_resetMethod;
            }

            set
            {
                if (!Object.ReferenceEquals(m_resetMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_resetMethod = value;
            }
        }

        /// <remarks />
        public BaseDataVariableState<OrientationDataType> Orientation
        {
            get
            {
                return m_orientation;
            }

            set
            {
                if (!Object.ReferenceEquals(m_orientation, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_orientation = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_temperature != null)
            {
                children.Add(m_temperature);
            }

            if (m_pressure != null)
            {
                children.Add(m_pressure);
            }

            if (m_resetMethod != null)
            {
                children.Add(m_resetMethod);
            }

            if (m_orientation != null)
            {
                children.Add(m_orientation);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Measurements.BrowseNames.Temperature:
                {
                    if (createOrReplace)
                    {
                        if (Temperature == null)
                        {
                            if (replacement == null)
                            {
                                Temperature = new AnalogUnitState<double>(this);
                            }
                            else
                            {
                                Temperature = (AnalogUnitState<double>)replacement;
                            }
                        }
                    }

                    instance = Temperature;
                    break;
                }

                case Measurements.BrowseNames.Pressure:
                {
                    if (createOrReplace)
                    {
                        if (Pressure == null)
                        {
                            if (replacement == null)
                            {
                                Pressure = new AnalogUnitState<double>(this);
                            }
                            else
                            {
                                Pressure = (AnalogUnitState<double>)replacement;
                            }
                        }
                    }

                    instance = Pressure;
                    break;
                }

                case Measurements.BrowseNames.Reset:
                {
                    if (createOrReplace)
                    {
                        if (Reset == null)
                        {
                            if (replacement == null)
                            {
                                Reset = new MeasurementResetMethodState(this);
                            }
                            else
                            {
                                Reset = (MeasurementResetMethodState)replacement;
                            }
                        }
                    }

                    instance = Reset;
                    break;
                }

                case Measurements.BrowseNames.Orientation:
                {
                    if (createOrReplace)
                    {
                        if (Orientation == null)
                        {
                            if (replacement == null)
                            {
                                Orientation = new BaseDataVariableState<OrientationDataType>(this);
                            }
                            else
                            {
                                Orientation = (BaseDataVariableState<OrientationDataType>)replacement;
                            }
                        }
                    }

                    instance = Orientation;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private AnalogUnitState<double> m_temperature;
        private AnalogUnitState<double> m_pressure;
        private MeasurementResetMethodState m_resetMethod;
        private BaseDataVariableState<OrientationDataType> m_orientation;
        #endregion
    }
    #endif
    #endregion

    #region MeasurementResetMethodState Class
    #if (!OPCUA_EXCLUDE_MeasurementResetMethodState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class MeasurementResetMethodState : MethodState
    {
        #region Constructors
        /// <remarks />
        public MeasurementResetMethodState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        public new static NodeState Construct(NodeState parent)
        {
            return new MeasurementResetMethodState(parent);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADUAAAB1cm46b3BjZm91bmRhdGlvbi5vcmc6MjAyNC0xMDpzdGFydGVya2l0Om1lYXN1cmVtZW50" +
           "c/////8EYYIKBAAAAAEAGgAAAE1lYXN1cmVtZW50UmVzZXRNZXRob2RUeXBlAQEcAAAvAQEcABwAAAAB" +
           "Af////8CAAAAF2CpCgIAAAAAAA4AAABJbnB1dEFyZ3VtZW50cwEBHQAALgBEHQAAAJYCAAAAAQAqAQEd" +
           "AAAADgAAAE5ld1RlbXBlcmF0dXJlAAv/////AAAAAAABACoBARoAAAALAAAATmV3UHJlc3N1cmUAC///" +
           "//8AAAAAAAEAKAEBAAAAAQAAAAIAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1bWVu" +
           "dHMBAR4AAC4ARB4AAACWAgAAAAEAKgEBHQAAAA4AAABPbGRUZW1wZXJhdHVyZQAL/////wAAAAAAAQAq" +
           "AQEaAAAACwAAAE9sZFByZXNzdXJlAAv/////AAAAAAABACgBAQAAAAEAAAACAAAAAQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Event Callbacks
        /// <remarks />
        public MeasurementResetMethodStateMethodCallHandler OnCall;
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        /// <remarks />
        protected override ServiceResult Call(
            ISystemContext _context,
            NodeId _objectId,
            IList<object> _inputArguments,
            IList<object> _outputArguments)
        {
            if (OnCall == null)
            {
                return base.Call(_context, _objectId, _inputArguments, _outputArguments);
            }

            ServiceResult _result = null;

            double newTemperature = (double)_inputArguments[0];
            double newPressure = (double)_inputArguments[1];

            double oldTemperature = (double)_outputArguments[0];
            double oldPressure = (double)_outputArguments[1];

            if (OnCall != null)
            {
                _result = OnCall(
                    _context,
                    this,
                    _objectId,
                    newTemperature,
                    newPressure,
                    ref oldTemperature,
                    ref oldPressure);
            }

            _outputArguments[0] = oldTemperature;
            _outputArguments[1] = oldPressure;

            return _result;
        }
        #endregion

        #region Private Fields
        #endregion
    }

    /// <remarks />
    /// <exclude />
    public delegate ServiceResult MeasurementResetMethodStateMethodCallHandler(
        ISystemContext _context,
        MethodState _method,
        NodeId _objectId,
        double newTemperature,
        double newPressure,
        ref double oldTemperature,
        ref double oldPressure);
    #endif
    #endregion
}