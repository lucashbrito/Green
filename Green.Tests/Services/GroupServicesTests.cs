using Green.Application.Services;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Moq;
using FluentAssertions;
using Green.Domain.Entities;
namespace Green.Tests.Services;

public class GroupServicesTests
{
    private Mock<IGroupRepository> _mockGroupRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    private GroupService _service;

    public GroupServicesTests()
    {
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _service = new GroupService(_mockGroupRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateGroup_ShouldCreateGroup_WhenCalledWithValidData()
    {
        // Act
        var result = await _service.CreateGroup("Test Group", 100);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Group");
        result.CapacityInAmps.Should().Be(100);
        _mockGroupRepository.Verify(m => m.Add(It.IsAny<Group>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateGroup_ShouldUpdateGroup_WhenCalledWithValidData()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Group("Test Group", 100);
        _mockGroupRepository.Setup(m => m.GetById(groupId)).ReturnsAsync(group);

        // Act
        await _service.UpdateGroup(groupId, "Updated Group", 200);

        // Assert
        group.Name.Should().Be("Updated Group");
        group.CapacityInAmps.Should().Be(200);
        _mockGroupRepository.Verify(m => m.Update(It.IsAny<Group>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveGroup_ShouldRemoveGroup_WhenCalledWithValidData()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Group("Test Group", 100);
        _mockGroupRepository.Setup(m => m.GetById(groupId)).ReturnsAsync(group);

        // Act
        await _service.RemoveGroup(groupId);

        // Assert
        _mockGroupRepository.Verify(m => m.Remove(It.IsAny<Group>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }
}
