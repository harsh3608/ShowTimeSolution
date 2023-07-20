﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShowTime.Core.IdentityEntities;
using ShowTime.Core.Models;
using ShowTime.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShowTime.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Generates a JWT token using the given user's information and the configuration settings.
        /// </summary>
        /// <param name="user">ApplicationUser object</param>
        /// <returns>AuthenticationResponse that includes token</returns>
        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            // Create a DateTime object representing the token expiration time by adding the number of minutes specified in the configuration to the current UTC time.
            DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

            // Create an array of Claim objects representing the user's claims, such as their ID, name, email, etc.
            Claim[] claims = new Claim[] {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject (user id)
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID
               new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), //Issued at (date and time of token generation)

               //Name, User Type
               new Claim("PersonName", user.PersonName),
               new Claim("Email", user.Email),
               new Claim("UserType", (user.UserType).ToString()),
               new Claim("JobRole", (user.JobRole).ToString())

            };

            // Create a SymmetricSecurityKey object using the key specified in the configuration.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create a SigningCredentials object with the security key and the HMACSHA256 algorithm.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a JwtSecurityToken object with the given issuer, audience, claims, expiration, and signing credentials.
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: signingCredentials
            );

            // Create a JwtSecurityTokenHandler object and use it to write the token as a string.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            // Create and return an AuthenticationResponse object containing the token, user email, user name, and token expiration time.
            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                PersonName = user.PersonName,
                UserType = user.UserType,
                Expiration = expiration,
                ManagerId = user.ManagerId,
                ManagerName = user.ManagerName
            };
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
            {
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),

                    ValidateLifetime = false //should be false
                };

                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
        }
    }
