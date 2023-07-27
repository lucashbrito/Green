using FluentAssertions;
using Green.Application.Group.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Moq;

namespace Green.Tests.Group;

public class GroupCommandHandlerTests
{
    private Mock<IGroupRepository> _mockGroupRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    public GroupCommandHandlerTests()
    {
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task CreateGroup_ShouldCreateGroup_WhenCalledWithValidData()
    {
        // Arrange
        var command = new CreateGroupCommand("Test Group", 100);
        var handler = new CreateGroupCommandHandler(_mockGroupRepository.Object, _mockUnitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Group");
        result.CapacityInAmps.Should().Be(100);
        _mockGroupRepository.Verify(m => m.Add(It.IsAny<Domain.Entities.    Group>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateGroup_ShouldUpdateGroup_WhenCalledWithValidData()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Domain.Entities.Group("Test Group", 100);
        _mockGroupRepository.Setup(m => m.GetById(groupId)).ReturnsAsync(group);

        var command = new UpdateGroupCommand(groupId, "Updated Group", 200);
        var handler = new UpdateGroupCommandHandler(_mockGroupRepository.Object, _mockUnitOfWork.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        group.Name.Should().Be("Updated Group");
        group.CapacityInAmps.Should().Be(200);
        _mockGroupRepository.Verify(m => m.Update(It.IsAny<Domain.Entities.Group>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveGroup_ShouldRemoveGroup_WhenCalledWithValidData()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Domain.Entities.Group("Test Group", 100);
        _mockGroupRepository.Setup(m => m.GetById(groupId)).ReturnsAsync(group);

        var command = new RemoveGroupCommand(groupId);
        var handler = new RemoveGroupCommandHandler(_mockGroupRepository.Object, _mockUnitOfWork.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockGroupRepository.Verify(m => m.Remove(It.IsAny<Domain.Entities.Group>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
