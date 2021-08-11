using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.unitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemsRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            /* -------------
            When you assert mocks, it actually verify something
            that happened to the mock accuracy test
            ---------------- */
            // var repositoryStub = new Mock<IItemsRepository>(); //comes from Moq Library
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null); // Casting to item entity

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act - where actions are executed
            var result = await  controller.GetItemAsync(Guid.NewGuid());
            
            //Assert
            //Assert.IsType<NotFoundResult>(result.Result); // Class represents NotFound
            result.Result.Should().BeOfType<NotFoundResult>();
        } 

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectedItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act - where actions are executed
            var result = await  controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItem_ReturnsAllItems()
        {
            // Arrange
            var expectedItems = new[]{CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            
            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var actualItems = await controller.GetItemsAsync();

            // Assert
            actualItems.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
        {
            // Arrange
            var allItems = new[]
            {
                new Item(){ Name = "Potion"},
                new Item(){ Name = "Antidote"},
                new Item(){ Name = "Hi-Potion"}    
            };

            var nameToMatch = "Potion";
            
            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(allItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            // Assert
            foundItems.Should().OnlyContain(
                item => item.Name == allItems[0].Name || item.Name == allItems[2].Name
            );
        }


        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto(
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
                rand.Next(1000));
                
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
             var existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                existingItem.Price + 3
            );

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            // Act
            var result = await controller.updateItemAsync(itemId, itemToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            
        }


        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            // Act
            var result = await controller.DeleteItemAsync(existingItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();

        }
   

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}


// use good naming convention for unit testing
// UnitOfWork_StateUnderTest_ExpenctedBehaviour()

/* Inside body 
    {
    // Arrange

    // Act

    //Assert
    }
*/


 /* ----- If we are not using Fluent Assertions----
            Assert.IsType<ItemDto>(result.Value);
            var dto = (result as ActionResult<ItemDto>).Value;
            Assert.Equal(expectedItem.Id, dto.Id);
            Assert.Equal(expectedItem.Name, dto.Name);
            Assert.Equal(expectedItem.Price, dto.Price);
            Assert.Equal(expectedItem.CreatedDate, dto.CreatedDate);

            -----------for record class--------
            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>()
            );
            --------------------------------------

            --------for normal class-----------
            result.Value.Should().BeEquivalentTo(expectedItem);
            --------------------------- */