using AutoFixture;
using AutoMapper;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.Contracts.Users;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Comments.Entity;
using BulletinBoard.Domain.Users.Entity;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace BulletinBoard.Tests.MapProfilesTests;

/// <summary>
/// Тесты для <see cref="UserProfile"/>.
/// </summary>
public class UserProfileTests
{
    private readonly MapperConfiguration _configurationProfider;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly FakeTimeProvider _timeProvider;
    
    public UserProfileTests()
    {
        _timeProvider = new FakeTimeProvider();
        _timeProvider.SetUtcNow(DateTime.UtcNow);
        _configurationProfider = new MapperConfiguration(delegate(IMapperConfigurationExpression configure)
        {
            configure.AddProfile(new UserProfile());
        });
        _fixture = new Fixture();
        _mapper = _configurationProfider.CreateMapper();
    }

    [Fact]
    public void UserProfile_CheckConfigurationIsValid()
    {
        _configurationProfider.AssertConfigurationIsValid();
    }

    [Fact]
    public void UserProfile_RegisterUserRequest_To_UserDto()
    {
        //Arrange
        var email = _fixture.Create<string>();
        var phone = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var name = _fixture.Create<string>();
        var createdAt = _timeProvider.GetUtcNow().DateTime;
        
        var source = _fixture
            .Build<RegisterUserRequest>()
            .With(x => x.Email, email)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .With(x => x.Name, name)
            .Create();
        
        //Act
        var result = _mapper.Map<UserDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            createdAt,
            email,
            phone,
            password,
            name
        },
            opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public void UserProfile_User_To_UserDto()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _fixture.Create<DateTime>();
        var email = _fixture.Create<string>();
        var phone = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var name = _fixture.Create<string>();
        var role = _fixture.Create<string>();

        var source = _fixture
            .Build<User>()
            .With(x => x.Email, email)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .With(x => x.Name, name)
            .With(x => x.Role, role)
            .With(x => x.Bulletins, new List<Bulletin>())
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.Comments, new List<Comment>())
            .Create();
        
        //Act
        var result = _mapper.Map<UserDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            id,
            createdAt,
            email,
            phone,
            password,
            name,
            role
        },
        opt => opt.ExcludingMissingMembers());
    }
    
    [Fact]
    public void UserProfile_UserDto_To_User()
    {
        //Arrange
        var id = _fixture.Create<Guid>();
        var createdAt = _fixture.Create<DateTime>();
        var email = _fixture.Create<string>();
        var phone = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var name = _fixture.Create<string>();
        var role = _fixture.Create<string>();

        var source = _fixture
            .Build<User>()
            .With(x => x.Email, email)
            .With(x => x.Phone, phone)
            .With(x => x.Password, password)
            .With(x => x.Name, name)
            .With(x => x.Role, role)
            .With(x => x.Bulletins, new List<Bulletin>())
            .With(x => x.Id, id)
            .With(x => x.CreatedAt, createdAt)
            .With(x => x.Comments, new List<Comment>())
            .Create();
        
        //Act
        var result = _mapper.Map<UserDto>(source);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
            {
                id,
                createdAt,
                email,
                phone,
                password,
                name,
                role
            },
            opt => opt.ExcludingMissingMembers());
    }
}