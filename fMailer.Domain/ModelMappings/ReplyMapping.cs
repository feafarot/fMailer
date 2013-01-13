namespace fMailer.Domain.ModelMappings
{
    using FluentNHibernate.Mapping;
    using fMailer.Domain.Model;

    public class ReplyMapping : ClassMap<Reply>
    {
        public ReplyMapping()
        {
            Id(x => x.Id);

            Map(x => x.RecievedOn).Not.Nullable();
            Map(x => x.EmailText).Not.Nullable().CustomSqlType("nvarchar(MAX)").Length(int.MaxValue);
            Map(x => x.IsNew).Not.Nullable().Default("1");
            Map(x => x.Subject).Not.Nullable();

            HasMany(x => x.Attachments).Cascade.All();

            References(x => x.From).Not.Nullable().Cascade.SaveUpdate();
            References(x => x.Distribution).Not.Nullable().Cascade.SaveUpdate();
        }
    }
}
