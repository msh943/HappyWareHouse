using AutoMapper;
using HappyWarehouse.Api.Controllers;
using HappyWarehouse.Domain.Dto;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Service.IService;
using HappyWarehouse.Service.Mapping;
using HappyWarehouse.Tests.Unit.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace HappyWarehouse.Tests.Unit.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _svc = new();
        private readonly IMapper _mapper;

        public UsersControllerTests()
        {
            _mapper = Tests.Helpers.MapperHelper.CreateMapper(typeof(MappingConfig));
        }

        [Fact]
        public async Task Update_Should_Block_Self_Deactivate()
        {

            var svc = new Mock<IUserService>();
            svc.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1, IsActive = true, RoleId = 2, Email = "e", FullName = "f" });

            var mapper = MapperHelper.CreateMapper(typeof(MappingConfig));
            var controller = new UsersController(svc.Object, mapper);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, "1"),

            }, authenticationType: "Test");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            var dto = new UpdateUserDto
            {
                Id = 1,
                Email = "e@e.com",
                FullName = "Name",
                RoleId = 2,
                IsActive = false
            };


            var result = await controller.Update(1, dto);


            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("You cannot deactivate your own account.", bad.Value);
        }
    }
}
