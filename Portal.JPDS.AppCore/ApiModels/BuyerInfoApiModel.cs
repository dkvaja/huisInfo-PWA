using Portal.JPDS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class BuyersInfoApiModel
    {
        public BuyersInfoApiModel() { }

        public BuyersInfoApiModel(KoperHuurder entity)
        {
            Type = entity.Soort;
            if (Type == 0)
            {
                P1 = new BuyerInfoApiModel
                {
                    LoginId = entity.Login.FirstOrDefault(x => x.PersoonGuid == entity.Persoon1Guid)?.Guid,
                    PersonId = entity.Persoon1Guid,
                    FirstName = entity.Persoon1Gu?.Voornaam,
                    LastName = entity.Persoon1Gu?.Achternaam,
                    Name = entity.Persoon1Gu?.Naam,
                    TelephonePrivate = entity.Persoon1Gu?.Telefoon,
                    Mobile = entity.Persoon1Gu?.Mobiel,
                    TelephoneWork = entity.Persoon1TelefoonWerk,
                    EmailPrivate = entity.Persoon1Gu?.EmailPrive,
                    EmailWork = entity.Persoon1EmailWerk
                };
                if (entity.Persoon2Gu != null)
                {
                    P2 = new BuyerInfoApiModel
                    {
                        LoginId = entity.Login.FirstOrDefault(x => x.PersoonGuid == entity.Persoon2Guid)?.Guid,
                        PersonId = entity.Persoon2Guid,
                        FirstName = entity.Persoon2Gu?.Voornaam,
                        LastName = entity.Persoon2Gu?.Achternaam,
                        Name = entity.Persoon2Gu?.Naam,
                        TelephonePrivate = entity.Persoon2Gu?.Telefoon,
                        Mobile = entity.Persoon2Gu?.Mobiel,
                        TelephoneWork = entity.Persoon2TelefoonWerk,
                        EmailPrivate = entity.Persoon2Gu?.EmailPrive,
                        EmailWork = entity.Persoon2EmailWerk
                    };
                }
            }
            else if (Type == 1 && entity.OrganisatieGu != null)
            {
                Org = new OrgInfoApiModel(entity.OrganisatieGu, entity.RelatieGu)
                {
                    LoginId = entity.Login.FirstOrDefault(x => x.RelatieGuid == entity.RelatieGuid)?.Guid,
                };
            }
        }
        public int Type { get; set; }
        public BuyerInfoApiModel P1 { get; set; }
        public BuyerInfoApiModel P2 { get; set; }
        public OrgInfoApiModel Org { get; set; }
    }

    public class BuyerInfoApiModel
    {
        public BuyerInfoApiModel()
        {

        }
        public string LoginId { get; set; }
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string TelephonePrivate { get; set; }
        public string Mobile { get; set; }
        public string TelephoneWork { get; set; }
        public string EmailPrivate { get; set; }
        public string EmailWork { get; set; }
    }

    public class OrgInfoApiModel
    {
        public OrgInfoApiModel()
        {
        }

        public OrgInfoApiModel(Organisatie entity, Relatie relation)
        {
            if (entity != null)
            {
                OrganisatonId = entity.Guid;
                Name = entity.NaamOnderdeel ?? string.Empty;
                Email = entity.Email;
                Telephone = entity.Telefoon;
                if (relation != null)
                {
                    RelationId = relation.Guid;
                    RelationName = relation.PersoonGu?.Naam == "N.v.t." ? relation.AfdelingGu?.Afdeling1 : relation.PersoonGu?.Naam ?? string.Empty;
                    RelationTelephone = relation.Doorkiesnummer;
                    RelationMobile = relation.Mobiel ?? string.Empty;
                    RelationEmail = relation.EmailZakelijk;
                    RelationPersonId = relation.PersoonGuid;
                    RelationFunctionName = relation.FunctieGu?.Functie1 ?? string.Empty;
                    RelationPersonSex = relation.PersoonGu?.Geslacht;
                }
            }
        }

        public string LoginId { get; set; }
        public string BuyerRenterId { get; set; }
        public string OrganisatonId { get; set; }
        public string RelationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string RelationName { get; set; }
        public string RelationTelephone { get; set; }
        public string RelationMobile { get; set; }
        public string RelationEmail { get; set; }
        public string RelationPersonId { get; set; }
        public byte? RelationPersonSex { get; set; }
        public string RelationFunctionName { get; set; }
    }
}
