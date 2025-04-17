using System;
using System.Collections.Generic;
using System.IO;

namespace Assessment2Task2
{
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    class Program
    {
        public static List<Room> listOfRooms = new List<Room>();
        public static List<RoomAllocation> listOfRoomAllocations = new List<RoomAllocation>();
        public static string filePath;

        static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(folderPath, "lhms_studentid.txt");
            string backupFilePath = Path.Combine(folderPath, "lhms_studentid_backup.txt");

            char ans;
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine(" LANGHAM HOTEL MANAGEMENT SYSTEM");
                    Console.WriteLine(" MENU");
                    Console.WriteLine("1. Add Rooms");
                    Console.WriteLine("2. Display Rooms");
                    Console.WriteLine("3. Allocate Rooms");
                    Console.WriteLine("4. De-Allocate Rooms");
                    Console.WriteLine("5. Display Room Allocation Details");
                    Console.WriteLine("6. Billing");
                    Console.WriteLine("7. Save the Room Allocations To a File");
                    Console.WriteLine("8. Show the Room Allocations From a File");
                    Console.WriteLine("9. Exit");
                    Console.WriteLine("0. Backup Room Allocation File");

                    Console.Write("Enter Your Choice Number Here: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            AddRooms();
                            break;
                        case 2:
                            DisplayRooms();
                            break;
                        case 3:
                            AllocateRoom();
                            break;
                        case 4:
                            DeallocateRoom();
                            break;
                        case 5:
                            DisplayRoomAllocations();
                            break;
                        case 6:
                            Console.WriteLine("Billing Feature is Under Construction and will be added soon...!!!");
                            break;
                        case 7:
                            SaveRoomAllocationsToFile(listOfRoomAllocations, filePath);
                            break;
                        case 8:
                            ShowRoomAllocationsFromFile(filePath);
                            break;
                        case 9:
                            return;
                        case 0:
                            BackupAndClearFile(filePath, backupFilePath);
                            break;
                        default:
                            Console.WriteLine("Invalid Choice!");
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Input format error: " + ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Operation error: " + ex.Message);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("File error: " + ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Access denied: " + ex.Message);
                    Console.WriteLine("File Path: " + filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }

                Console.Write("\nWould You Like To Continue(Y/N): ");
                ans = Convert.ToChar(Console.ReadLine());
            } while (ans == 'y' || ans == 'Y');
        }

        public static void AddRooms()
        {
            try
            {
                Console.Write("Enter number of rooms to add: ");
                int n = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    Console.Write("Enter Room Number: ");
                    int roomNo = Convert.ToInt32(Console.ReadLine());

                    if (listOfRooms.Exists(r => r.RoomNo == roomNo))
                        throw new InvalidOperationException("Room number already exists.");

                    listOfRooms.Add(new Room { RoomNo = roomNo, IsAllocated = false });
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid input. Please enter numeric values only. " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Operation error: " + ex.Message);
            }
        }

        public static void DisplayRooms()
        {
            if (listOfRooms.Count == 0)
            {
                Console.WriteLine("No rooms available.");
                return;
            }

            foreach (var room in listOfRooms)
            {
                Console.WriteLine($"Room No: {room.RoomNo}, Allocated: {room.IsAllocated}");
            }
        }

        public static void AllocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to Allocate: ");
                int roomNo = Convert.ToInt32(Console.ReadLine());

                var room = listOfRooms.Find(r => r.RoomNo == roomNo);
                if (room == null)
                    throw new InvalidOperationException("Room does not exist.");
                if (room.IsAllocated)
                    throw new InvalidOperationException("Room is already allocated.");

                Console.Write("Enter Customer Number: ");
                int custNo = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter Customer Name: ");
                string custName = Console.ReadLine();

                room.IsAllocated = true;
                listOfRoomAllocations.Add(new RoomAllocation
                {
                    AllocatedRoomNo = room.RoomNo,
                    AllocatedCustomer = new Customer { CustomerNo = custNo, CustomerName = custName }
                });

                Console.WriteLine("Room Allocated Successfully!");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid input. Please enter numeric values only. " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Operation error: " + ex.Message);
            }
        }

        public static void DeallocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to De-Allocate: ");
                int roomNo = Convert.ToInt32(Console.ReadLine());

                var room = listOfRooms.Find(r => r.RoomNo == roomNo);
                if (room == null || !room.IsAllocated)
                    throw new InvalidOperationException("Room not allocated or does not exist.");

                room.IsAllocated = false;
                listOfRoomAllocations.RemoveAll(a => a.AllocatedRoomNo == roomNo);
                Console.WriteLine("Room De-Allocated Successfully!");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid input. Please enter numeric values only. " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Operation error: " + ex.Message);
            }
        }

        public static void DisplayRoomAllocations()
        {
            if (listOfRoomAllocations.Count == 0)
            {
                Console.WriteLine("No room allocations to display.");
                return;
            }

            foreach (var alloc in listOfRoomAllocations)
            {
                Console.WriteLine($"Room {alloc.AllocatedRoomNo} is allocated to Customer {alloc.AllocatedCustomer.CustomerNo}, {alloc.AllocatedCustomer.CustomerName}");
            }
        }

        public static void SaveRoomAllocationsToFile(List<RoomAllocation> allocations, string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    foreach (var allocation in allocations)
                    {
                        sw.WriteLine($"{DateTime.Now}: Room {allocation.AllocatedRoomNo} -> Customer {allocation.AllocatedCustomer.CustomerNo}, {allocation.AllocatedCustomer.CustomerName}");
                    }
                }
                Console.WriteLine("Room Allocations Saved To File.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access denied: You may not have permission to write to this file. " + ex.Message);
                Console.WriteLine("File Path: " + filePath);
            }
        }

        public static void ShowRoomAllocationsFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("The room allocation file does not exist.");

                string[] contents = File.ReadAllLines(filePath);
                foreach (var line in contents)
                {
                    Console.WriteLine(line);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File error: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access denied while reading the file: " + ex.Message);
            }
        }

        public static void BackupAndClearFile(string filePath, string backupFilePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("No records to backup. File not found.");

                File.AppendAllText(backupFilePath, File.ReadAllText(filePath));
                File.WriteAllText(filePath, string.Empty);
                Console.WriteLine("Backup completed and original file cleared.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access denied while backing up or clearing the file. " + ex.Message);
                Console.WriteLine("File Path: " + filePath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File error: " + ex.Message);
            }
        }
    }
}
