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
namespace OpenApi.Opc.Ua
{
    public static class Attributes
    {
        public const long NodeId = 1;
        public const long NodeClass = 2;
        public const long BrowseName = 3;
        public const long DisplayName = 4;
        public const long Description = 5;
        public const long WriteMask = 6;
        public const long UserWriteMask = 7;
        public const long IsAbstract = 8;
        public const long Symmetric = 9;
        public const long InverseName = 10;
        public const long ContainsNoLoops = 11;
        public const long EventNotifier = 12;
        public const long Value = 13;
        public const long DataType = 14;
        public const long ValueRank = 15;
        public const long ArrayDimensions = 16;
        public const long AccessLevel = 17;
        public const long UserAccessLevel = 18;
        public const long MinimumSamplingInterval = 19;
        public const long Historizing = 20;
        public const long Executable = 21;
        public const long UserExecutable = 22;
        public const long DataTypeDefinition = 23;
        public const long RolePermissions = 24;
        public const long UserRolePermissions = 25;
        public const long AccessRestrictions = 26;
        public const long AccessLevelEx = 27;

        public static string ToName(long value)
        {
            return typeof(Attributes)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(x => x.GetValue(null).Equals(value))
                .Select(x => x.Name)
                .FirstOrDefault();
        }
    }
    
}
