using System.Collections.Generic;
using EvidencijaSoftvera_IlijaDivljan.DAL;
using EvidencijaSoftvera_IlijaDivljan.Models;
using EvidencijaSoftvera_IlijaDivljan.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvidencijaSoftvera_IlijaDivljan.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EvidencijaSoftvera_IlijaDivljan.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EvidencijaSoftvera_IlijaDivljan.DAL.ApplicationDbContext context)
        {
            AddUserAndRole(context, "Admin", "dnilija@live.com", "Ilija", "Divljan", "123456");
            AddUserAndRole(context, "User", "marko@gmail.com", "Marko", "Markovic", "123456");
            AddUserAndRole(context, "User", "nikola@gmail.com", "Nikola", "Ninkovic", "123456");
            AddUserAndRole(context, "User", "ana@gmail.com", "Ana", "Ninkovic", "123456");
            AddUserAndRole(context, "Admin", "mdanijel@gmail.com", "Danijel", "Mijic", "123456");
            context.SaveChanges();

            //Popunjavnaje liste dodatne opreme u tabeli AdditionalEquipment
            var additionalEq = new List<AdditionalEquipment>
            {
                new AdditionalEquipment {Name = "Monitor",Description = "N/A"},
                new AdditionalEquipment {Name = "Mouse",Description = "N/A"},
                new AdditionalEquipment {Name = "Keyboard",Description = "N/A"},
                new AdditionalEquipment {Name = "Web Cam",Description = "N/A"},
                new AdditionalEquipment {Name = "Speakers",Description = "N/A"},
                new AdditionalEquipment {Name = "Headphones",Description = "N/A"},
                new AdditionalEquipment {Name = "Printer",Description = "N/A"}
            };
            additionalEq.ForEach(s => context.AdditionalEquipment.AddOrUpdate(a => a.Name, s));
            context.SaveChanges();

            //Popunjavanje liste za softver
            var software = new List<Software>
            {
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 10 Enterprise (x64)", Version = "Enterprise",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 10 Enterprise (x86)", Version = "Enterprise",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 10 Education N (x64)", Version = "Education",
                    License = LicenseEnum.Free
                },
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 8.1 Pro (x64)", Version = "Pro",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 8 Pro (x86)", Version = "Pro",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 7 Home (x64)", Version = "Home",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.OperatingSystem, Manufacturer = "Microsoft", Name = "Windows 7 Home (x86)", Version = "Home",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.BusinessSoftware, Manufacturer = "Microsoft", Name = "Office Professional Plus 2013 with Service Pack 1 (x64)", Version = "Professional Plus",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.BusinessSoftware, Manufacturer = "Microsoft", Name = "Office Professional 2013 (x86)", Version = "Professional",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.DeveloperTools, Manufacturer = "Microsoft", Name = "Visual Studio Enterprise 2015 (x86 and x64)", Version = "Enterprise",
                    License = LicenseEnum.Commercial
                },
                new Software
                {
                    Category = SoftwareEnum.DeveloperTools, Manufacturer = "Microsoft", Name = "Visual Studio Enterprise 2013 (x86 and x64)", Version = "Enterprise",
                    License = LicenseEnum.Commercial
                }
            };
            software.ForEach(s => context.Software.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            //Popunjavanje liste za racunare.
            var computers = new List<Computers>
            {
                new Computers
                {
                    Name = "S1_P1_R1", ComputerType = ComputerEnum.Laptop, Manufacturer = "Dell", Model = "Dell Latitude E6410", SerialNumber = "113A09SH322", Cpu = "Intel i5 520M", Ram = 8, VideoCard = "Intel HD graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "dnilija@live.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S1_P1_R2", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "2413A0LA2122", Cpu = "Intel i7", Ram = 8, VideoCard = "Intel HD graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "dnilija@live.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S1_P1_R3", ComputerType = ComputerEnum.Desktop, Manufacturer = "Apple", Model = "Apple - 21.5\" iMac", SerialNumber = "213SDMN122", Cpu = "Intel Core i5 (2.7GHz)", Ram = 8, VideoCard = "Intel HD graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S1_P1_R4", ComputerType = ComputerEnum.Desktop, Manufacturer = "Apple", Model = "Apple - 21.5\" iMac", SerialNumber = "313A09SD323", Cpu = "Intel Core i5 (2.7GHz)", Ram = 8, VideoCard = "Intel HD graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S1_P1_R5", ComputerType = ComputerEnum.Desktop, Manufacturer = "Apple", Model = "Apple - 21.5\" iMac", SerialNumber = "113A0SDN322", Cpu = "Intel Core i5 (2.7GHz)", Ram = 8, VideoCard = "Intel HD graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S1_P1_R6", ComputerType = ComputerEnum.Desktop, Manufacturer = "Apple", Model = "Apple - 21.5\" iMac", SerialNumber = "123A09MD232", Cpu = "Intel Core i5 (2.7GHz)", Ram = 8, VideoCard = "Intel HD graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S2_P1_R1", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "063A0FM232", Cpu = "Intel i7", Ram = 8, VideoCard = "AMD Radeon HD 7700m Series Graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S2_P1_R2", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "413P4FM2122", Cpu = "Intel i7", Ram = 8, VideoCard = "AMD Radeon HD 7700m Series Graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                    
                },
                new Computers
                {
                    Name = "S2_P2_R1", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "413A0KF2122", Cpu = "Intel i7", Ram = 8, VideoCard = "AMD Radeon HD 7700m Series Graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S2_P2_R2", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "413A0S32122", Cpu = "Intel i7", Ram = 8, VideoCard = "AMD Radeon HD 7700m Series Graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S2_P2_R3", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "413A0KL2122", Cpu = "Intel i7", Ram = 8, VideoCard = "AMD Radeon HD 7700m Series Graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                },
                new Computers
                {
                    Name = "S2_P2_R4", ComputerType = ComputerEnum.Desktop, Manufacturer = "Dell", Model = "Dell Inspiron I3847-6162BK", SerialNumber = "4112A0FM2122", Cpu = "Intel i7", Ram = 8, VideoCard = "AMD Radeon HD 7700m Series Graphics",
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id,
                    AdditionalEquipment = new List<AdditionalEquipment>()
                }
            };
            computers.ForEach(c => context.Computers.AddOrUpdate(s => s.SerialNumber, c));
            context.SaveChanges();
            
            //Popunjavanje tabele koja nastaje kao rezultat many to many veze
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R1", "Monitor");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R2", "Monitor");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R3", "Monitor");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R1", "Speakers");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R2", "Speakers");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R3", "Speakers");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R1", "Mouse");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R2", "Mouse");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R3", "Mouse");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R1", "Keyboard");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R2", "Keyboard");
            AddOrUpdateComputersAndEquipment(context, "S1_P1_R3", "Keyboard");
            context.SaveChanges();

            //Popunjavanje instaliranog softvera
            var installedSoftware = new List<InstalledSoftware>
            {
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-09-01"),
                    ComputersId = computers.Single(c => c.Name == "S1_P1_R1").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Windows 10 Enterprise (x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "dnilija@live.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-09-01"),
                    ComputersId = computers.Single(c => c.Name == "S1_P1_R1").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Office Professional Plus 2013 with Service Pack 1 (x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "dnilija@live.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-09-01"),
                    ComputersId = computers.Single(c => c.Name == "S1_P1_R1").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Visual Studio Enterprise 2015 (x86 and x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "dnilija@live.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-08-29"),
                    ComputersId = computers.Single(c => c.Name == "S1_P1_R3").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Windows 10 Enterprise (x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-08-29"),
                    ComputersId = computers.Single(c => c.Name == "S1_P1_R3").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Office Professional 2013 (x86)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-08-29"),
                    ComputersId = computers.Single(c => c.Name == "S1_P1_R3").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Visual Studio Enterprise 2013 (x86 and x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "marko@gmail.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-08-24"),
                    ComputersId = computers.Single(c => c.Name == "S2_P1_R1").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Windows 7 Home (x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-08-24"),
                    ComputersId = computers.Single(c => c.Name == "S2_P1_R1").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Office Professional 2013 (x86)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id
                },
                new InstalledSoftware
                {
                    RecordDate = DateTime.Parse("2015-08-24"),
                    ComputersId = computers.Single(c => c.Name == "S2_P1_R1").ComputersId,
                    SoftwareId = software.Single(s => s.Name == "Visual Studio Enterprise 2013 (x86 and x64)").SoftwareId,
                    ApplicationUserId = context.Users.Single(u => u.UserName == "nikola@gmail.com").Id
                }
            };

            foreach (InstalledSoftware i in installedSoftware)
            {
                var installedSoftwareInDb = context.InstalledSoftware
                    .SingleOrDefault(s => s.ComputersId == i.ComputersId && s.SoftwareId == i.SoftwareId);

                if (installedSoftwareInDb == null)
                {
                    context.InstalledSoftware.Add(i);
                }
            }
            context.SaveChanges();
        }

        // Popunjavamo bazu koja se dobija kao many to many relationship izmedju Computers i AdditionalEquipment
        private void AddOrUpdateComputersAndEquipment(ApplicationDbContext context, string computerName,
            string additionalEquipment)
        {
            var comp = context.Computers.SingleOrDefault(c => c.Name == computerName);
            if (comp != null)
            {
                var adEq = comp.AdditionalEquipment.SingleOrDefault(a => a.Name == additionalEquipment);

                if (adEq == null)
                {
                    comp.AdditionalEquipment.Add(context.AdditionalEquipment.Single(a => a.Name == additionalEquipment));
                }
            }
        }

        private bool AddUserAndRole(ApplicationDbContext context, string role, string email, string firstName,
            string lastName, string password)
        {
            IdentityResult ir;

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            ir = roleManager.Create(new IdentityRole(role));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            ir = userManager.Create(user, password);

            if (ir.Succeeded == false)
            {
                return ir.Succeeded;
            }

            ir = userManager.AddToRole(user.Id, role);
            return ir.Succeeded;
        }
    }
}