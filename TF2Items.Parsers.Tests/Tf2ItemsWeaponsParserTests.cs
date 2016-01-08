﻿using System.Collections.Generic;
using log4net.Config;
using NUnit.Framework;
using TF2Items.Core;

namespace TF2Items.Parsers.Tests
{
    [TestFixture]
    public class Tf2ItemsWeaponsParserTests
    {
        [SetUp]
        public void Setup()
        {
            BasicConfigurator.Configure();
        }

        [Test]
        public void ParsesConfig1()
        {
            ServerConfiguration expectedConfig = new ServerConfiguration
                                                 {
                                                     WeaponCollections = new List<WeaponCollection>
                                                                         {
                                                                             new WeaponCollection
                                                                             {
                                                                                 Users = UserIdentifier.FromStreamIds("STEAM_0:1:13776935", "STEAM_0:1:17919459"),
                                                                                 Weapons = new List<Weapon>
                                                                                           {
                                                                                               new Weapon(WeaponIdentifier.FromId(12))
                                                                                               {
                                                                                                   Quality = 5,
                                                                                                   Level = 100,
                                                                                                   AdminFlags = "a",
                                                                                                   Attributes = new List<WeaponAttribute>
                                                                                                                {
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(134,   2),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  2, 100),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  4,  10),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  6,   .25f),
                                                                                                                }
                                                                                               }
                                                                                           }
                                                                             }
                                                                         }
                                                 };
            string filepath = TestDeployment.GetFile("tf2items.weapons.1.txt");
            ServerConfiguration config = new Tf2ItemsWeaponsParser().Parse(filepath);

            AssertEquality(config, expectedConfig);
        }

        [Test]
        public void ParsesExampleConfig()
        {
            ServerConfiguration expectedConfig = new ServerConfiguration
                                                 {
                                                     WeaponCollections = new List<WeaponCollection>
                                                                         {
                                                                             new WeaponCollection
                                                                             {
                                                                                 Users = UserIdentifier.FromStreamIds("STEAM_0:1:13776935", "STEAM_0:1:17919459"),
                                                                                 Weapons = new List<Weapon>
                                                                                           {
                                                                                               new Weapon(WeaponIdentifier.FromId(12))
                                                                                               {
                                                                                                   Quality = 5,
                                                                                                   Level = 100,
                                                                                                   AdminFlags = "a",
                                                                                                   Attributes = new List<WeaponAttribute>
                                                                                                                {
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(134,   2),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  2, 100),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  4,  10),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  6,   .25f),
                                                                                                                }
                                                                                               },
                                                                                               new Weapon(WeaponIdentifier.FromId(12))
                                                                                               {
                                                                                                   Quality = 4,
                                                                                                   Level = 50,
                                                                                               },
                                                                                               new Weapon(WeaponIdentifier.FromId(18))
                                                                                               {
                                                                                                   Quality = 1,
                                                                                                   Level = 100,
                                                                                                   Attributes = new List<WeaponAttribute>
                                                                                                                {
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(134,   2),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  2, 100),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  4,  10),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  6,   .25f),
                                                                                                                }
                                                                                               },
                                                                                               new Weapon(WeaponIdentifier.FromId(45))
                                                                                               {
                                                                                                   Quality = 3,
                                                                                                   Level = 10,
                                                                                                   Attributes = new List<WeaponAttribute>
                                                                                                                {
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(134,   2),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  2, 100),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  4,  10),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  6,   .25f),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons( 16, 500),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons( 26, 250),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons( 31,  10),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(123,   3),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(134,   4),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(  3,   .17f),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons( 97,   .6f),
                                                                                                                    WeaponAttribute.FromTf2ItemsWeapons(106,   .5f),
                                                                                                                }
                                                                                               },
                                                                                               new Weapon(WeaponIdentifier.FromId(4))
                                                                                               {
                                                                                                   Quality = 9,
                                                                                                   Level = 10,
                                                                                               },
                                                                                               new Weapon(WeaponIdentifier.Any())
                                                                                               {
                                                                                                   Quality = 6,
                                                                                               },
                                                                                           }
                                                                             },
                                                                             new WeaponCollection
                                                                             {
                                                                                 Weapons = new List<Weapon>
                                                                                           {
                                                                                               new Weapon(WeaponIdentifier.FromId(4))
                                                                                               {
                                                                                                   Quality =  2,
                                                                                                   Level = 50,
                                                                                               },
                                                                                               new Weapon(WeaponIdentifier.Any())
                                                                                               {
                                                                                                   AdminFlags = "z",
                                                                                                   Quality = 6
                                                                                               },
                                                                                           }
                                                                             }
                                                                         }
                                                 };
            string filepath = TestDeployment.GetFile("tf2items.weapons.example.txt");
            ServerConfiguration config = new Tf2ItemsWeaponsParser().Parse(filepath);

            AssertEquality(config, expectedConfig);
        }

        private void AssertEquality(ServerConfiguration actualConfig, ServerConfiguration expectedConfig)
        {
            Assert.That(actualConfig.WeaponCollections.Count, Is.EqualTo(expectedConfig.WeaponCollections.Count), "WeaponCollections.Count");


            for (int i = 0; i < actualConfig.WeaponCollections.Count; i++)
            {
                WeaponCollection actualWeaponCollection = actualConfig.WeaponCollections[i];
                WeaponCollection expectedWeaponCollection = expectedConfig.WeaponCollections[i];
                AssertEquality(actualWeaponCollection, expectedWeaponCollection);
            }
        }

        private void AssertEquality(WeaponCollection actualWeaponCollection, WeaponCollection expectedWeaponCollection)
        {
            Assert.That(actualWeaponCollection.Users, Is.EqualTo(expectedWeaponCollection.Users), "WeaponCollection.Users");

            for (int i = 0; i < actualWeaponCollection.Weapons.Count; i++)
            {
                Weapon actualWeapon = actualWeaponCollection.Weapons[i];
                Weapon expectedWeapon = expectedWeaponCollection.Weapons[i];
                AssertEquality(actualWeapon, expectedWeapon);
            }
        }

        private void AssertEquality(Weapon actualWeapon, Weapon expectedWeapon)
        {
            Assert.That(actualWeapon.Id,            Is.EqualTo(expectedWeapon.Id),          "Weapon.Id");
            Assert.That(actualWeapon.Name,          Is.EqualTo(expectedWeapon.Name),        "Weapon.Name");
            Assert.That(actualWeapon.Quality,       Is.EqualTo(expectedWeapon.Quality),     "Weapon.Quality");
            Assert.That(actualWeapon.Level,         Is.EqualTo(expectedWeapon.Level),       "Weapon.Level");
            Assert.That(actualWeapon.AdminFlags,    Is.EqualTo(expectedWeapon.AdminFlags),  "Weapon.AdminFlags");

            for (int i = 0; i < actualWeapon.Attributes.Count; i++)
            {
                WeaponAttribute actualAttribute = actualWeapon.Attributes[i];
                WeaponAttribute expectedAttribute = expectedWeapon.Attributes[i];
                AssertEquality(actualAttribute, expectedAttribute);
            }
        }

        private void AssertEquality(WeaponAttribute actualAttribute, WeaponAttribute expectedAttribute)
        {
            Assert.That(actualAttribute.Id,         Is.EqualTo(expectedAttribute.Id),           "expectedAttribute.Id");
            Assert.That(actualAttribute.Name,       Is.EqualTo(expectedAttribute.Name),         "expectedAttribute.Name");
            Assert.That(actualAttribute.Value,      Is.EqualTo(expectedAttribute.Value),        "expectedAttribute.Value");
            Assert.That(actualAttribute.EffectType, Is.EqualTo(expectedAttribute.EffectType),   "expectedAttribute.EffectType");
            Assert.That(actualAttribute.Format,     Is.EqualTo(expectedAttribute.Format),       "expectedAttribute.Format");
        }
    }
}
