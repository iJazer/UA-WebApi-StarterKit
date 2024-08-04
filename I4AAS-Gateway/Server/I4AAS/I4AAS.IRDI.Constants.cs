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
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace I4AAS.IRDI
{
    #region Object Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Objects
    {
        /// <remarks />
        public const string IRDI_0173_1_02_AAY811_001 = "0173-1#02-AAY811#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO677_002 = "0173-1#02-AAO677#002";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_1_0_ContactInformations_ContactInformation = "https://admin-shell.io/zvei/nameplate/1/0/ContactInformations/ContactInformation";

        /// <remarks />
        public const string IRDI_0173_1_02_AAW338_001 = "0173-1#02-AAW338#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAU732_001 = "0173-1#02-AAU732#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAU731_001 = "0173-1#02-AAU731#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO057_002 = "0173-1#02-AAO057#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO227_002 = "0173-1#02-AAO227#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO676_003 = "0173-1#02-AAO676#003";

        /// <remarks />
        public const string IRDI_0173_1_02_AAM556_002 = "0173-1#02-AAM556#002";

        /// <remarks />
        public const string IRDI_0173_1_2_AAP906_001 = "0173-1#02-AAP906#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAR972_002 = "0173-1#02-AAR972#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAN270_002 = "0173-1#02-AAN270#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAM985_002 = "0173-1#02-AAM985#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAM737_002 = "0173-1#02-AAM737#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO259_004 = "0173-1#02-AAO259#004";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_CompanyLogo = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/CompanyLogo";

        /// <remarks />
        public const string IRDI_0173_1_01_AGZ673_001 = "0173-1#01-AGZ673#001";

        /// <remarks />
        public const string IRDI_0173_1_01_AHD206_001 = "0173-1#01-AHD206#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO128_002 = "0173-1#02-AAO128#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO129_002 = "0173-1#02-AAO129#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO132_002 = "0173-1#02-AAO132#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO134_002 = "0173-1#02-AAO134#002";

        /// <remarks />
        public const string IRDI_0112_2___61987_ABH783_001 = "0112/2///61987#ABH783#001";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingName = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/MarkingName";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_IssueDate = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/IssueDate";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_ExpiryDate = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/ExpiryDate";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingFile = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/MarkingFile";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingAdditionalText = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/MarkingAdditionalText";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG854_001 = "0173-1#02-ABG854#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG855_001 = "0173-1#02-ABG855#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG856_001 = "0173-1#02-ABG856#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG857_001 = "0173-1#02-ABG857#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG858_001 = "0173-1#02-ABG858#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABI497_001 = "0173-1#02-ABI497#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH956_001 = "0173-1#02-ABH956#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH957_001 = "0173-1#02-ABH957#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH958_001 = "0173-1#02-ABH958#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH959_001 = "0173-1#02-ABH959#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO259_005 = "0173-1#02-AAO259#005";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH960_001 = "0173-1#02-ABH960#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH961_001 = "0173-1#02-ABH961#001";
    }
    #endregion

    #region Object Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAY811_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAY811_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO677_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO677_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_1_0_ContactInformations_ContactInformation = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_1_0_ContactInformations_ContactInformation, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAW338_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAW338_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAU732_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAU732_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAU731_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAU731_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO057_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO057_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO227_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO227_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO676_003 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO676_003, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAM556_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAM556_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_2_AAP906_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_2_AAP906_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAR972_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAR972_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAN270_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAN270_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAM985_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAM985_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAM737_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAM737_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO259_004 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO259_004, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_CompanyLogo = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_CompanyLogo, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_01_AGZ673_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_01_AGZ673_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_01_AHD206_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_01_AHD206_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO128_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO128_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO129_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO129_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO132_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO132_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO134_002 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO134_002, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0112_2___61987_ABH783_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0112_2___61987_ABH783_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingName = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingName, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_IssueDate = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_IssueDate, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_ExpiryDate = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_ExpiryDate, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingFile = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingFile, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingAdditionalText = new ExpandedNodeId(I4AAS.IRDI.Objects.IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingAdditionalText, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABG854_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABG854_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABG855_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABG855_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABG856_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABG856_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABG857_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABG857_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABG858_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABG858_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABI497_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABI497_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABH956_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABH956_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABH957_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABH957_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABH958_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABH958_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABH959_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABH959_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_AAO259_005 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_AAO259_005, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABH960_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABH960_001, I4AAS.IRDI.Namespaces.IRDI);

        /// <remarks />
        public static readonly ExpandedNodeId IRDI_0173_1_02_ABH961_001 = new ExpandedNodeId(I4AAS.IRDI.Objects.IRDI_0173_1_02_ABH961_001, I4AAS.IRDI.Namespaces.IRDI);
    }
    #endregion

    #region BrowseName Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
    {
        /// <remarks />
        public const string IRDI_0112_2___61987_ABH783_001 = "0112/2///61987#ABH783#001";

        /// <remarks />
        public const string IRDI_0173_1_01_AGZ673_001 = "0173-1#01-AGZ673#001";

        /// <remarks />
        public const string IRDI_0173_1_01_AHD206_001 = "0173-1#01-AHD206#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAM556_002 = "0173-1#02-AAM556#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAM737_002 = "0173-1#02-AAM737#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAM985_002 = "0173-1#02-AAM985#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAN270_002 = "0173-1#02-AAN270#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO057_002 = "0173-1#02-AAO057#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO128_002 = "0173-1#02-AAO128#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO129_002 = "0173-1#02-AAO129#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO132_002 = "0173-1#02-AAO132#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO134_002 = "0173-1#02-AAO134#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO227_002 = "0173-1#02-AAO227#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO259_004 = "0173-1#02-AAO259#004";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO259_005 = "0173-1#02-AAO259#005";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO676_003 = "0173-1#02-AAO676#003";

        /// <remarks />
        public const string IRDI_0173_1_02_AAO677_002 = "0173-1#02-AAO677#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAR972_002 = "0173-1#02-AAR972#002";

        /// <remarks />
        public const string IRDI_0173_1_02_AAU731_001 = "0173-1#02-AAU731#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAU732_001 = "0173-1#02-AAU732#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAW338_001 = "0173-1#02-AAW338#001";

        /// <remarks />
        public const string IRDI_0173_1_02_AAY811_001 = "0173-1#02-AAY811#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG854_001 = "0173-1#02-ABG854#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG855_001 = "0173-1#02-ABG855#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG856_001 = "0173-1#02-ABG856#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG857_001 = "0173-1#02-ABG857#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABG858_001 = "0173-1#02-ABG858#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH956_001 = "0173-1#02-ABH956#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH957_001 = "0173-1#02-ABH957#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH958_001 = "0173-1#02-ABH958#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH959_001 = "0173-1#02-ABH959#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH960_001 = "0173-1#02-ABH960#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABH961_001 = "0173-1#02-ABH961#001";

        /// <remarks />
        public const string IRDI_0173_1_02_ABI497_001 = "0173-1#02-ABI497#001";

        /// <remarks />
        public const string IRDI_0173_1_2_AAP906_001 = "0173-1#02-AAP906#001";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_1_0_ContactInformations_ContactInformation = "https://admin-shell.io/zvei/nameplate/1/0/ContactInformations/ContactInformation";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_CompanyLogo = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/CompanyLogo";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_ExpiryDate = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/ExpiryDate";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_IssueDate = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/IssueDate";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingAdditionalText = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/MarkingAdditionalText";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingFile = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/MarkingFile";

        /// <remarks />
        public const string IRI_admin_shell_io_zvei_nameplate_2_0_Nameplate_Markings_Marking_MarkingName = "https://admin-shell.io/zvei/nameplate/2/0/Nameplate/Markings/Marking/MarkingName";
    }
    #endregion

    #region Namespace Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the IRDI namespace (.NET code namespace is 'I4AAS.IRDI').
        /// </summary>
        public const string IRDI = "http://opcfoundation.org/UA/Dictionary/IRDI";

        /// <summary>
        /// The URI for the IRDIXsd namespace (.NET code namespace is 'I4AAS.IRDI').
        /// </summary>
        public const string IRDIXsd = "http://opcfoundation.org/UA/Dictionary/IRDI/Types.xsd";

        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";
    }
    #endregion
}