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
            Map(x => x.EmailText).Not.Nullable();
            References(x => x.From).Cascade.None();
        }
    }
}
