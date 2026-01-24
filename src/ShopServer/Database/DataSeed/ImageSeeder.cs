using Microsoft.EntityFrameworkCore;
using Contracts.Images.Entities;

namespace Database.DataSeed;

public class ImageSeeder : ISeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>().HasData(CreateData());
    }

    private static List<Image> CreateData()
    {
        return
        [
            new Image
            {
                Id = 1,
                Name = "p1-tent-aura-2-ultralight",
                ProductId = 1
            },
            new Image
            {
                Id = 2,
                Name = "p2-tent-citadel-fortress-2",
                ProductId = 2
            },
            new Image
            {
                Id = 3,
                Name = "p2-tent-citadel-fortress-2-background",
                ProductId = 2
            },
            new Image
            {
                Id = 4,
                Name = "p3-tent-citadel-fortress-4",
                ProductId = 3
            },
            new Image
            {
                Id = 5,
                Name = "p4-shoes-vanguard-low-pro",
                ProductId = 4
            },
            new Image
            {
                Id = 6,
                Name = "p5-shoes-titan-claw-boots",
                ProductId = 5
            },

            new Image
            {
                Id = 7,
                Name = "p6-backpack-atlas-hauler-75l-blue",
                ProductId = 6
            },
            new Image
            {
                Id = 8,
                Name = "p6-backpack-atlas-hauler-75l-braun",
                ProductId = 6
            },
            new Image
            {
                Id = 9,
                Name = "p6-backpack-atlas-hauler-75l-green",
                ProductId = 6
            },
            new Image
            {
                Id = 10,
                Name = "p7-backpack-swift-pulse-15l",
                ProductId = 7
            },
            new Image
            {
                Id = 11,
                Name = "p8-backpack-nomad-versa-pack",
                ProductId = 8
            }
        ];
    }
}