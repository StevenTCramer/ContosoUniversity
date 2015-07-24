namespace ContosoUniversity.Features.Student
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Student, Index.Result.Student>();
            CreateMap<Student, Details.Model>();
            CreateMap<Enrollment, Details.Model.Enrollment>();
            CreateMap<Create.Command, Student>(MemberList.Source);
            CreateMap<Student, Edit.Command>().ReverseMap();
            CreateMap<Student, Delete.Command>().ReverseMap();
        }
    }
}