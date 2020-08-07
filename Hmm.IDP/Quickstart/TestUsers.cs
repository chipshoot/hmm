// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };
                
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "61C66F3B-271B-49A5-A899-3B01A6B51D7F",
                        Username = "fchy",
                        Password = "shcdlhgm",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Chaoyang Fang"),
                            new Claim(JwtClaimTypes.GivenName, "Chaoyang"),
                            new Claim(JwtClaimTypes.FamilyName, "Fang"),
                            new Claim(JwtClaimTypes.Email, "fchy@yahoo.com"),
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "157BBC69-9989-4353-A4B9-02A205678562",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        }
                    }
                };
            }
        }
    }
}