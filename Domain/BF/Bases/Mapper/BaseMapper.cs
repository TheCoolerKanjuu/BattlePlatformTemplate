using AutoMapper;
using Common.BF.Bases;
using Infrastructure.BF.Bases;

namespace Domain.BF.Bases.Mapper;

/// <summary>
/// Base class of all mappers. Its purpose is to convert a <see cref="TDto"/> into a <see cref="TEntity"/> and vice-versa.
/// </summary>
/// <typeparam name="TDto">The DTO part of the mapper.></typeparam>
/// <typeparam name="TEntity">The Entity part of the mapper.</typeparam>
public class BaseMapper<TDto, TEntity>
    where TDto: BaseDto
    where TEntity : BaseEntity
{
    /// <summary>
    /// Convert a <see cref="TDto"/> into a <see cref="TEntity"/>.
    /// </summary>
    /// <param name="dto">the <see cref="TDto"/> to be converted.</param>
    /// <returns>A <see cref="TEntity"/></returns>
    public TEntity DtoToEntity(TDto dto)
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<TDto, TEntity>());
        var mapper = config.CreateMapper();
        return mapper.Map<TEntity>(dto);
    }

    /// <summary>
    /// Convert a <see cref="TDto"/> into a <see cref="TEntity"/>.
    /// </summary>
    /// <param name="entity">the <see cref="TEntity"/> to be converted.</param>
    /// <returns>A <see cref="TEntity"/></returns>
    public TDto EntityToDto(TEntity entity)
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TDto>());
        var mapper = config.CreateMapper();
        return mapper.Map<TDto>(entity);
    }
}