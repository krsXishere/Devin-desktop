using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Devin
{
    internal class Engine
    {
        public String GetData(String hwClass, String syntax)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "select * from " + "Win32_" + hwClass);

            foreach (ManagementObject obj in searcher.Get())
            {
                if(obj != null)
                {
                    return Convert.ToString(obj[syntax]);
                }
            }
            throw new Exception();
        }

        public ulong GetPhysicalMemoryCapacity()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "select * from " + "Win32_PhysicalMemory");

            ulong installedMemory = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                installedMemory += (UInt64)obj["Capacity"];
            }

            return installedMemory;
        }

        public ulong GetLogicalDiskCapacity()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "select * from " + "Win32_LogicalDisk");

            ulong installedMemory = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                return installedMemory += (UInt64)obj["Size"];
            }

            throw new Exception();
        }

        public ulong GetDiskDriveCapacity()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "select * from " + "Win32_DiskDrive");

            ulong installedMemory = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                return installedMemory += (UInt64)obj["Size"];
            }

            throw new Exception();
        }
    }
}
