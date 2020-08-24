// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace Hmm.IDP.Quickstart
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
                        SubjectId = "6E8FDCED-8857-46B9-BAD8-6DF2540FD07E",
                        Username = "fchy",
                        Password = "fchy",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Chaoyang Fang"),
                            new Claim(JwtClaimTypes.GivenName, "Chaoyang"),
                            new Claim(JwtClaimTypes.FamilyName, "Fang"),
                            new Claim(JwtClaimTypes.Email, "fchy@yahoo.com"),
                            new Claim(JwtClaimTypes.Address, "1750 Bloor St."),
                            new Claim(JwtClaimTypes.Role, "author")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "1501CAB6-CA3F-470F-AE5E-1A0B970D1707",
                        Username = "fzt",
                        Password = "fzt",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Zhitao Fang"),
                            new Claim(JwtClaimTypes.GivenName, "Zhitao"),
                            new Claim(JwtClaimTypes.FamilyName, "Fang"),
                            new Claim(JwtClaimTypes.Email, "taobaobao@gmail.com"),
                            new Claim(JwtClaimTypes.Address, "29 Spencer Ave."),
                            new Claim(JwtClaimTypes.Role, "author"),
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
                            new Claim(JwtClaimTypes.Address, "3345 Cardross Rd."),
                            new Claim(JwtClaimTypes.Role, "guest")
                        }
                    }
                };
            }
        }
    }
}