using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Core.Manager;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.TestHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class GasLogManagerTests : TestFixtureBase
    {
        private readonly IGasLogManager _manager;

        public GasLogManagerTests()
        {
            var authors = new List<User>
            {
                new User
                {
                    FirstName = "Jack",
                    LastName = "Fang",
                    AccountName = "jfang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                },
                new User
                {
                    FirstName = "Amy",
                    LastName = "Wang",
                    AccountName = "awang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                }
            };

            var renders = new List<NoteRender>
            {
                new NoteRender
                {
                    Name = "DefaultNoteRender",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                },
                new NoteRender
                {
                    Name = "GasLog",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                }
            };

            var catalogs = new List<NoteCatalog>
            {
                new NoteCatalog
                {
                    Name = "DefaultNoteCatalog",
                    Render = renders[0],
                    Schema = "<?xml version=\"1.0\" encoding=\"UTF-16\"?><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:vc=\"http://www.w3.org/2007/XMLSchema-versioning\" xmlns:hmm=\"http://schema.hmm.com/2017\" targetNamespace=\"http://schema.hmm.com/2017\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\" vc:minVersion=\"1.1\"><xs:element name=\"Note\"><xs:annotation><xs:documentation>The root of all note managed by HMM</xs:documentation></xs:annotation><xs:complexType><xs:sequence><xs:element name=\"Content\" type=\"xs:string\"/></xs:sequence></xs:complexType></xs:element></xs:schema>",
                    Description = "Testing catalog"
                },

                new NoteCatalog
                {
                    Name = "Gas Log",
                    Render = renders[1],
                    Schema = "<?xml version=\"1.0\" encoding=\"UTF-16\"?><xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:vc=\"http://www.w3.org/2007/XMLSchema-versioning\" xmlns:rns=\"http://schema.hmm.com/2017\" targetNamespace=\"http://schema.hmm.com/2017\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\" vc:minVersion=\"1.1\"><xs:element name=\"Note\"><xs:annotation><xs:documentation>The root of all note managed by HMM</xs:documentation></xs:annotation><xs:complexType><xs:sequence><xs:element name=\"Content\"><xs:complexType><xs:sequence><xs:element name=\"GasLog\"><xs:annotation><xs:documentation>Comment describing your root element</xs:documentation></xs:annotation><xs:complexType><xs:sequence><xs:element name=\"Date\" type=\"xs:dateTime\"/><xs:element name=\"Distance\" type=\"rns:DimensionType\"/><xs:element name=\"Gas\"><xs:complexType><xs:complexContent><xs:extension base=\"rns:VolumeType\"/></xs:complexContent></xs:complexType></xs:element><xs:element name=\"Price\" type=\"rns:MonetaryType\"/><xs:element name=\"GasStation\" type=\"xs:string\"/><xs:element name=\"Discounts\"><xs:complexType><xs:sequence><xs:element name=\"Discount\" type=\"rns:DiscountType\" minOccurs=\"0\" maxOccurs=\"unbounded\"/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element><xs:complexType name=\"DimensionType\"><xs:sequence><xs:element name=\"Dimension\"><xs:complexType><xs:sequence><xs:element name=\"Value\" type=\"xs:double\"/><xs:element name=\"Unit\" type=\"xs:string\"/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType><xs:complexType name=\"VolumeType\"><xs:sequence><xs:element name=\"Volume\"><xs:complexType><xs:sequence><xs:element name=\"Value\"/><xs:element name=\"Unit\"/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType><xs:complexType name=\"MonetaryType\"><xs:sequence><xs:element name=\"Money\"><xs:complexType><xs:sequence><xs:element name=\"Value\" type=\"xs:decimal\"/><xs:element name=\"Code\" type=\"xs:string\"/></xs:sequence></xs:complexType></xs:element></xs:sequence></xs:complexType><xs:complexType name=\"DiscountType\"><xs:sequence><xs:element name=\"Amount\" type=\"rns:MonetaryType\"/><xs:element name=\"Program\" type=\"xs:string\"/></xs:sequence></xs:complexType></xs:schema>",
                    Description = "Testing catalog"
                }
            };

            SetupRecords(authors, renders, catalogs);
            var noteManager = new HmmNoteManager(NoteStorage, LookupRepo);
            _manager = new GasLogManager(noteManager, LookupRepo);
        }

        [Fact]
        public void CanAddGasLog()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var catalog = CatalogStorage.GetEntities().FirstOrDefault(cat => cat.Name == "Gas Log");
            var gasLog = new GasLog
            {
                Author = user,
                Catalog = catalog,
                GasStation = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300),
                CreateDate = DateTime.UtcNow,
                Discounts = new List<GasDiscountInfo>
                {
                    new GasDiscountInfo
                    {
                        Amount = new Money(0.8),
                        Program = "Patrol Canada RBC connection"
                    }
                }
            };

            // Act
            var newGas = _manager.CreateLog(gasLog);

            // Assert
            Assert.True(newGas.Id >= 1, "newGas.Id >= 1");
            Assert.True(_manager.ProcessResult.Success);
        }

        //        [Fact]
        //        public void CanUpdateGasLog()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        [Fact]
        //        public void CanDeleteCasLog()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        [Fact]
        //        public void CanFindGasLog()
        //        {
        //            throw new NotImplementedException();
        //        }
    }
}