using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{

    class Device
        {
            public int DeviceId { get; }
            public string Name { get; }
            public Failure FailureType { get; }
            public DateTime DateFail { get; }

            public Device(int deviceId, string deviceName, Failure failureType, DateTime dateFail)
            {
                DeviceId = deviceId;
                Name = deviceName;
                FailureType = failureType;
                DateFail = dateFail;
            }

        }

    class Failure
    {
        int Fail { get; }

        public Failure(int x)
        { Fail = x; }

        public static explicit operator Failure(int p1)
        {
            return new Failure(p1);
        }
        // серьезный отказ
        public bool IsFailureSerious()
        {
            if (Fail % 2 == 0) return true;
            return false;
        }
    }

       
       
    public class Common
    {   

        // сравнение двух дат
        public static int Earlier(object[] v, int day, int month, int year)
        {
            int vYear = (int)v[2];
            int vMonth = (int)v[1];
            int vDay = (int)v[0];
            if (vYear < year) return 1;
            if (vYear > year) return 0;
            if (vMonth < month) return 1;
            if (vMonth > month) return 0;
            if (vDay < day) return 1;
            return 0;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown,  // для неожиданного отключения
        /// 1 for short non-responding, // для коротких не отвечающих
        /// 2 for hardware failures,    // для аппаратных сбоев
        /// 3 for connection problems   // для проблем подключения
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        /// 
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            DateTime date = new DateTime(year, month, day);
            List<Device> devicesAll = new List<Device>();
            // преобразовали входные данные
            for (int i = 0; i < failureTypes.Length; i++)
            {
                Device device = new Device(deviceId[0], (string)devices[0]["Name"], (Failure)failureTypes[0], new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]));
                devicesAll.Add(device);
            }
            // вызываем новый метод
            return FindDevicesFailedBeforeDate(date, devicesAll);
            /*
            // HashSet - множество элементов, содержит только уникальные не сортированные объекты
            var problematicDevices = new HashSet<int>();
            for (int i = 0; i < failureTypes.Length; i++)
                if (Common.IsFailureSerious(failureTypes[i])==1 && Common.Earlier(times[i], day, month, year)==1)
                    problematicDevices.Add(deviceId[i]);

            var result = new List<string>();
            foreach (var device in devices)
                if (problematicDevices.Contains((int)device["DeviceId"]))
                    result.Add(device["Name"] as string);

            return result;
            */
        }

        static List<string> FindDevicesFailedBeforeDate(DateTime date, List<Device> devices)
        {
            var result = new List<string>();

            foreach (var dev in devices)
            {
                if (dev.FailureType.IsFailureSerious() && dev.DateFail.CompareTo(date) <= 0)
                    result.Add(dev.Name);
            }
            return result;
        }

    }
}
