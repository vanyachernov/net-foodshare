using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodshare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert existing string categories to enum values
            // Map common category names to enum values:
            // Other = 0, MainCourse = 1, Soup = 2, Salad = 3, Dessert = 4, Bakery = 5, Snack = 6
            
            migrationBuilder.Sql(@"
                ALTER TABLE dishes 
                ALTER COLUMN category TYPE integer 
                USING (
                    CASE 
                        WHEN LOWER(category) LIKE '%main%' OR LOWER(category) LIKE '%основн%' THEN 1
                        WHEN LOWER(category) LIKE '%soup%' OR LOWER(category) LIKE '%суп%' THEN 2
                        WHEN LOWER(category) LIKE '%salad%' OR LOWER(category) LIKE '%салат%' THEN 3
                        WHEN LOWER(category) LIKE '%dessert%' OR LOWER(category) LIKE '%десерт%' THEN 4
                        WHEN LOWER(category) LIKE '%bak%' OR LOWER(category) LIKE '%випічк%' THEN 5
                        WHEN LOWER(category) LIKE '%snack%' OR LOWER(category) LIKE '%закуск%' THEN 6
                        ELSE 0
                    END
                )::integer
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Convert enum values back to string categories
            migrationBuilder.Sql(@"
                ALTER TABLE dishes 
                ALTER COLUMN category TYPE character varying(100) 
                USING (
                    CASE category
                        WHEN 1 THEN 'MainCourse'
                        WHEN 2 THEN 'Soup'
                        WHEN 3 THEN 'Salad'
                        WHEN 4 THEN 'Dessert'
                        WHEN 5 THEN 'Bakery'
                        WHEN 6 THEN 'Snack'
                        ELSE 'Other'
                    END
                )::character varying(100)
            ");
        }
    }
}
