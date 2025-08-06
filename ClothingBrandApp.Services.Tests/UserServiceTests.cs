using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Admin;
using ClothingBrandApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;


namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private Mock<IManagerRepository> managerRepositoryMock;
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>().Object;
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>().Object;
            var errorDescriber = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>().Object;
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<ApplicationUser>>>().Object;

            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errorDescriber,
                services,
                logger
            );

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleValidators = new List<IRoleValidator<IdentityRole>>();
            var roleKeyNormalizer = new Mock<ILookupNormalizer>().Object;
            var roleErrorDescriber = new IdentityErrorDescriber();
            var roleLogger = new Mock<Microsoft.Extensions.Logging.ILogger<RoleManager<IdentityRole>>>().Object;

            roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object,
                roleValidators,
                roleKeyNormalizer,
                roleErrorDescriber,
                roleLogger
            );

            managerRepositoryMock = new Mock<IManagerRepository>();

            userService = new UserService(userManagerMock.Object, roleManagerMock.Object, managerRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        //1.GetUserManagementBoardDataAsync

        [Test]
        public async Task GetUserManagementBoardDataAsync_ReturnsUsersExceptCurrent()
        {
            var users = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = "1", Email = "a@a.com" },
                    new ApplicationUser { Id = "2", Email = "b@b.com" }
                }.BuildMock();

            userManagerMock.SetupGet(x => x.Users).Returns(users);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "User" });

            var result = await userService.GetUserManagementBoardDataAsync("1");

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("2"));
            Assert.That(result.First().Roles, Contains.Item("User"));
        }

        //2.GetManagerEmailsAsync
        [Test]
        public async Task GetManagerEmailsAsync_ReturnsEmails()
        {
            var managers = new List<Manager>
            {
                new Manager { User = new ApplicationUser { UserName = "manager1" } },
                new Manager { User = new ApplicationUser { UserName = "manager2" } }
            }.BuildMock();

            managerRepositoryMock.Setup(x => x.GetAllAttached()).Returns(managers);

            var result = await userService.GetManagerEmailsAsync();

            Assert.That(result, Is.EquivalentTo(new[] { "manager1", "manager2" }));
        }

        [Test]
        public async Task GetManagerEmailsAsync_SkipsNullUserName()
        {
            var managers = new List<Manager>
            {
                new Manager { User = new ApplicationUser { UserName = null } },
                new Manager { User = new ApplicationUser { UserName = "manager2" } }
            }.BuildMock();

            managerRepositoryMock.Setup(x => x.GetAllAttached()).Returns(managers);

            var result = await userService.GetManagerEmailsAsync();

            Assert.That(result, Is.EquivalentTo(new[] { "manager2" }));
        }

        //3. AssignUserToRoleAsync

        [Test]
        public void AssignUserToRoleAsync_UserNotFound_Throws()
        {
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            Assert.ThrowsAsync<ArgumentException>(() => userService.AssignUserToRoleAsync(input));
        }

        [Test]
        public void AssignUserToRoleAsync_RoleNotExists_Throws()
        {
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            Assert.ThrowsAsync<ArgumentException>(() => userService.AssignUserToRoleAsync(input));
        }

        [Test]
        public async Task AssignUserToRoleAsync_ManagerRole_AddsManagerIfNotExists()
        {
            var user = new ApplicationUser { Id = "1" };

            var emptyManagers = new List<Manager>().BuildMock();

            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("Manager")).ReturnsAsync(true);

            // Return async IQueryable here, not just AsQueryable()
            managerRepositoryMock.Setup(x => x.GetAllAttached()).Returns(emptyManagers);

            managerRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Manager>())).Returns(Task.CompletedTask);
            userManagerMock.Setup(x => x.AddToRoleAsync(user, "Manager")).ReturnsAsync(IdentityResult.Success);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            var result = await userService.AssignUserToRoleAsync(input);

            Assert.That(result, Is.True);
            managerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Manager>()), Times.Once);
        }

        [Test]
        public async Task AssignUserToRoleAsync_ManagerRole_RevivesDeletedManager()
        {
            var user = new ApplicationUser { Id = "1" };
            var deletedManager = new Manager { UserId = "1", IsDeleted = true };

            var managers = new List<Manager> { deletedManager }.BuildMock();

            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("Manager")).ReturnsAsync(true);
            managerRepositoryMock.Setup(x => x.GetAllAttached()).Returns(managers);
            managerRepositoryMock.Setup(x => x.UpdateAsync(deletedManager)).ReturnsAsync(true);
            userManagerMock.Setup(x => x.AddToRoleAsync(user, "Manager")).ReturnsAsync(IdentityResult.Success);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            var result = await userService.AssignUserToRoleAsync(input);

            Assert.That(result, Is.True);
            Assert.That(deletedManager.IsDeleted, Is.False);
            managerRepositoryMock.Verify(x => x.UpdateAsync(deletedManager), Times.Once);
        }

        [Test]
        public async Task AssignUserToRoleAsync_NonManagerRole_Success()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("User")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.AddToRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "User" };

            var result = await userService.AssignUserToRoleAsync(input);

            Assert.That(result, Is.True);
        }

        [Test]
        public void AssignUserToRoleAsync_AddToRoleThrows_Throws()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("User")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.AddToRoleAsync(user, "User")).ThrowsAsync(new Exception("fail"));

            var input = new RoleSelectionInputModel { UserId = "1", Role = "User" };

            Assert.ThrowsAsync<ArgumentException>(() => userService.AssignUserToRoleAsync(input));
        }

        //4. RemoveUserFromRoleAsync
        [Test]
        public void RemoveUserFromRoleAsync_UserNotFound_Throws()
        {
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            Assert.ThrowsAsync<ArgumentException>(() => userService.RemoveUserFromRoleAsync(input));
        }

        [Test]
        public void RemoveUserFromRoleAsync_RoleNotExists_Throws()
        {
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            Assert.ThrowsAsync<ArgumentException>(() => userService.RemoveUserFromRoleAsync(input));
        }

        [Test]
        public void RemoveUserFromRoleAsync_UserNotInRole_Throws()
        {
            var user = new ApplicationUser();
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Manager")).ReturnsAsync(false);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            Assert.ThrowsAsync<InvalidOperationException>(() => userService.RemoveUserFromRoleAsync(input));
        }

        [Test]
        public async Task RemoveUserFromRoleAsync_ManagerRole_DeletesManager()
        {
            var user = new ApplicationUser { Id = "1" };
            var manager = new Manager { UserId = "1", IsDeleted = false };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("Manager")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Manager")).ReturnsAsync(true);
            managerRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Manager, bool>>>())).ReturnsAsync(manager);
            managerRepositoryMock.Setup(x => x.UpdateAsync(manager)).ReturnsAsync(true);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(user, "Manager")).ReturnsAsync(IdentityResult.Success);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "Manager" };

            var result = await userService.RemoveUserFromRoleAsync(input);

            Assert.That(result, Is.True);
            Assert.That(manager.IsDeleted, Is.True);
        }

        [Test]
        public async Task RemoveUserFromRoleAsync_NonManagerRole_Success()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("User")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "User")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);

            var input = new RoleSelectionInputModel { UserId = "1", Role = "User" };

            var result = await userService.RemoveUserFromRoleAsync(input);

            Assert.That(result, Is.True);
        }

        [Test]
        public void RemoveUserFromRoleAsync_RemoveFromRoleThrows_Throws()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            roleManagerMock.Setup(x => x.RoleExistsAsync("User")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "User")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(user, "User")).ThrowsAsync(new Exception("fail"));

            var input = new RoleSelectionInputModel { UserId = "1", Role = "User" };

            Assert.ThrowsAsync<InvalidOperationException>(() => userService.RemoveUserFromRoleAsync(input));
        }

        //5. DeleteUserAsync
        [Test]
        public void DeleteUserAsync_UserNotFound_Throws()
        {
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<ArgumentException>(() => userService.DeleteUserAsync("1"));
        }

        [Test]
        public void DeleteUserAsync_UserNotAdmin_Throws()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<InvalidOperationException>(() => userService.DeleteUserAsync("1"));
        }

        [Test]
        public void DeleteUserAsync_DeleteThrows_Throws()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.DeleteAsync(user)).ThrowsAsync(new Exception("fail"));

            Assert.ThrowsAsync<InvalidOperationException>(() => userService.DeleteUserAsync("1"));
        }

        [Test]
        public async Task DeleteUserAsync_DeleteSucceeds_ReturnsTrue()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await userService.DeleteUserAsync("1");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteUserAsync_DeleteFails_ReturnsFalse()
        {
            var user = new ApplicationUser { Id = "1" };
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed());

            var result = await userService.DeleteUserAsync("1");

            Assert.That(result, Is.False);
        }
    }
}