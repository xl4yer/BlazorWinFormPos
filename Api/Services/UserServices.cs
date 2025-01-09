﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Api.Class;
using Api.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MySql.Data.MySqlClient;

namespace Api.Services
{
    public class UserServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;
        private readonly AppSettings _appSetting;

        public UserServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
            _appSetting = appSettings.Value;
        }
        public async Task<List<users>> UserLogin(string username, string password)
        {
            List<users> xuser = new List<users>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("UserLogin", con)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                com.Parameters.Clear();
                com.Parameters.AddWithValue("_username", username);
                com.Parameters.AddWithValue("_password", password);

                var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                if (await rdr.ReadAsync().ConfigureAwait(false))
                {
                    xuser.Add(new users
                    {
                        userID = Convert.ToInt32(rdr["userID"]),
                        name = rdr["name"].ToString(),
                        username = rdr["username"].ToString(),
                        role = rdr["role"].ToString(),
                        password = rdr["password"].ToString()
                    });

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, xuser[0].username),
                    new Claim(ClaimTypes.Role, xuser[0].role!), // Ensure role is included
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.Aes128CbcHmacSha256) // Use HmacSha256 for better security
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    xuser[0].token = tokenHandler.WriteToken(token);
                }
                else
                {
                    xuser.Add(new users
                    {
                        userID = 0,
                        role = null,
                        name = null,
                        username = null,
                        password = null
                    });
                }
            }
            return xuser;
        }


        public async Task<List<users>> GetUsers()
        {
            List<users> u = new List<users>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("GetUsers", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        u.Add(new users
                        {
                            userID = Convert.ToInt32(rdr["userID"]),
                            name = rdr["name"].ToString(),
                            username = rdr["username"].ToString(),
                            password = rdr["password"].ToString(),
                            role = rdr["role"].ToString()
                        });
                    }
                    await rdr.CloseAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return u;
        }

        public async Task<int> AddUser(users u)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("AddUser", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_name", u.name);
                    com.Parameters.AddWithValue("_username", u.username);
                    com.Parameters.AddWithValue("_password", u.password);
                    com.Parameters.AddWithValue("_role", u.role);
                    return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return 0;
        }

        public async Task<int> UpdateUser(users u)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("UpdateUser", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_userID", u.userID);
                    com.Parameters.AddWithValue("_name", u.name);
                    com.Parameters.AddWithValue("_username", u.username);
                    com.Parameters.AddWithValue("_password", u.password);
                    com.Parameters.AddWithValue("_role", u.role);
                    return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return 0;
        }

        public async Task<List<users>> SearchUser(string search)
        {
            List<users> u = new List<users>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("SearchUser", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.Clear();
                    com.Parameters.AddWithValue("search", search);
                    com.Parameters.AddWithValue("@searchWildcard", $"{search}%");
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        u.Add(new users
                        {
                            userID = Convert.ToInt32(rdr["userID"]),
                            name = rdr["name"].ToString(),
                            username = rdr["username"].ToString(),
                            password = rdr["password"].ToString(),
                            role = rdr["role"].ToString()

                        });
                    }
                    await rdr.CloseAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return u;
        }
    }
}
