using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sp_GetAllPunchedInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            var sp = @"
USE [ShowTimeDatabase]
GO
/****** Object:  StoredProcedure [dbo].[GetAllPunchedInUsers]     Script Date: 7/4/2023 10:34:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Patel
-- Create date: 04/07/2023
-- Description:	Stored Procedure to Get All Punched In Users
-- =============================================
-- exec [dbo].[GetAllPunchedInUsers] 
CREATE PROCEDURE  [dbo].[GetAllPunchedInUsers] 
	
AS
BEGIN

SET NOCOUNT ON;

	SELECT p.*
        FROM [ShowTimeDatabase].[dbo].[Punches] p
        INNER JOIN (
            SELECT UserId, MAX(PunchDateTime) AS LatestPunchTime
            FROM [ShowTimeDatabase].[dbo].[Punches]
            WHERE CONVERT(date, PunchDateTime) = CONVERT(date, GETDATE())
            GROUP BY UserId
        ) latest ON p.UserId = latest.UserId AND p.PunchDateTime = latest.LatestPunchTime
        WHERE p.PunchStatus = 1;

END
";

            migrationBuilder.Sql(sp);
        }
    

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
