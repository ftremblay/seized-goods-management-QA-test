# Analyste en assurance qualité - Test technique 

## Setup
Installez le SDK, ainsi que le Runtime de DotNet Core 2.1 https://dotnet.microsoft.com/download/dotnet-core

Pour compiler le programme:
```sh
C:\SeizedGoodsManagementTest
> dotnet build
```
Pour rouler la suite de test:
```sh
C:\SeizedGoodsManagementTest\SeizedGoodsManagement.Tests
> dotnet test
```

## Mise en contexte 
En tant que développeur chez Emergensys, vous travaillez sur une application gérant la voûte policière de plusieurs postes de police au Québec. Une voûte policière est un endroit hautement sécurisé à l’intérieur de certains postes de police qui a comme mandat d’entreposer les objets saisis lors d’interventions.  

Les objets saisis se trouvant dans la voûte peuvent être de plusieurs types différents (drogue, armes, vêtement, nourriture, cigarettes, etc). Lors de l’arrivé d’un objet dans la voûte, une date de rétention est attribuée à celui-ci pour indiquer quand l’objet en question devra être disposé ou remis au propriétaire. 

Votre Product Owner vous donne la tâche de contribuer à l'implémentation et aux tests de la User Story suivante:

## User Story 101 - Mise à jour de la date de rétention
#### Description
En tant qu'enquêteur, je veux que la date de rétention d'un objet soit mise à jour,
lorsque celui-ci revient d'une demande d'expertise ou d'un test d'adn en laboratoire.

#### Critères d'acceptation
La date de rétention doit être mise à jour comme suit:
- Si le code de disposition est solutionné, qu'il y a adn positive et que la date de création + le délai solutionné pour adn < la date du jour + 6 mois: Date de rétention = Date du jour + 6 mois.
- Si le code de disposition est solutionné, qu'il y a adn positive et que la date de création + le délai solutionné pour adn >= la date du jour + 6 mois: Date de rétention = Date de création + le délai solutionné pour adn.
- Si le code de disposition est solutionné, qu'il n'y a pas adn positive et que la date de création + le délai solutionné par défaut < la date du jour + 6 mois: Date de rétention = Date du jour + 6 mois.
- Si le code de disposition est solutionné, qu'il n'y a pas adn positive et que la date de création + le délai solutionné par défaut >= la date du jour + 6 mois: Date de rétention = Date de création + le délai solutionné par défaut
- Si le code de disposition n'est pas solutionné et que le code de disposition est "A": Date de rétention = Date du jour + 6 mois.
- Si le code de disposition n'est pas solutionné, que le code de disposition est "B", que la demande d'expertise est négative, que les tests adn sont négatifs et que la date de création + le délai non solutionné par défaut < Date du jour + 6 mois: Date de rétention = Date du jour + 6 mois.
- Si le code de disposition n'est pas solutionné, que le code de disposition est "B", que la demande d'expertise est négative, que les tests adn négatifs et que la date de création + le délai non solutionné >= Date du jour + 6 mois: Date de rétention = Date de création + le délai non-solutionné par défaut.
- Si le code de disposition n'est pas solutionné, que le code de disposition est "B", que la demande d'expertise ou les tests adn sont positifs et que la date de création + le délai non-solutionné expertise < Date du jour + 6 mois: Date de rétention = Date du jour + 6 mois.
- Si le code de disposition n'est pas solutionné, que le code de disposition est "B", que la demande d'expertise ou les tests adn sont positifs et que la date de création + le délai non-solutionné expertise >= Date du jour + 6 mois: Date de rétention = Date de création + le délai non-solutionné expertise.
- Dans tous les autres cas: Date de rétention = Date de création + 6 mois.

#### Informations supplémentaires
Pour ce test, on vous demande seulement de coder la fonction "ComputeRetentionDate" dans la classe "RetentionDateService.cs" et de produire une suite de tests automatisés avec NUnit dans la classe "RetentionDateServiceTests.cs".

Vous pouvez décider de migrer tout le code fourni vers une solution dans un autre langage parmis ceux-ci: C#, F#, Java, TypeScript.

Vous pouvez utiliser les outils ou librairies avec lesquelles vous êtes habitué.

Vous êtes libre d'ajouter et modifier le code fournis à votre guise. C'est l'occasion de montrer ce que vous savez faire. 

Have fun!

## Code fourni
Pour ce test technique, nous vous fournissons une solution Dot Net Core 2.1 incluant deux projets classlib soit "SeizedGoodsManagement": contenant le code applicatif et "SeizedGoodsManagement.Tests" contenant la suite de tests unitaires.

La classe "DispositionCode.cs" permet de définir un code de disposition. Le code de disposition permet au préposé de la voûte de savoir comment disposer l'objet lorsque celui-ci atteindra sa date de rétention.
```csharp
public class DispositionCode
{
    public long Code { get; set; }
    public string Description { get; set; }
    public bool IsSolved { get; set; }
}
```

La classe "NatureCode.cs" permet de définir un code de nature. Le code nature est ce qui identifie la nature d'un événement. Par exemple, un meurtre au premier degrée, un vol, violence conjugale, etc.
```csharp
public class NatureCode
{
    public long Code { get; set; }
    public string Description {get;set;}
}
```

La classe "Item.cs" permet de définir un objet entreposé dans la voûte. 
- "IsDNAPositive": Ce booléen indique si l'analyse d'adn s'est avéré positive suite à des tests en laboratoire.
- "IsExpertisePositive": Ce booléen indique si une analyse demandant une expertise particulière s'est avéré positive.
- "DispositionCode": Contient les informations du code de disposition.
- "NatureCode": Contient les informations du code de nature.
- "CreationDate": Il s'agit de la date d'entrée de l'objet dans la voûte.
```csharp
public class Item
{
    public bool IsDNAPositive { get; set; }
    public bool IsExpertisePositive { get; set; }
    public DispositionCode DispositionCode { get; set; }
    public NatureCode NatureCode { get; set; }

    public DateTime CreationDate { get; set; }
}
```

L'interface "IDatabase.cs" déclare des fonctions permettant de retourner différents délais spécifiques en nombre de mois en lien avec un code de nature.
- "GetSolDNADelayForNatureCode": Retourne le délai solutionné lors d'une analyse d'adn positive selon le code de nature.
- "GetSolDelayForNatureCode": Retourne le délai solutionné par défaut selon le code de nature.
- "GetNonSolDelayForNatureCode": Retourne le délai non-solutionné par défaut selon le code de nature.
- "GetNonSolExpertiseDelayForNatureCode": Retourne le délai non-solutionné lors d'une expertise positive selon le code de nature.
```csharp
public interface IDatabase
    {
        float GetSolDNADelayForNatureCode(long natureCode);
        float GetSolDelayForNatureCode(long natureCode);
        float GetNonSolDelayForNatureCode(long natureCode);
        float GetNonSolExpertiseDelayForNatureCode(long natureCode);
    }
```

La classe "RetentionDateService.cs" expose une méthode permettant de calculer la date de rétention d'un objet. C'est dans cette classe que vous aurez à coder votre algorithme.
```csharp
public class RetentionDateService
{
    public DateTime ComputeRetentionDate(IDatabase database, Item item)
    {
        //The algorithm goes here

        //In every other cases, return item's creation date + 6 months
        return item.CreationDate.AddMonths(6);
    }
}
```

## Échéance
L'échéance vous sera transmis par courriel. Envoyez-nous votre solution zippé par courriel au xforce@emergensys.net.

## Questions
N'hésitez pas à nous poser vos questions au xforce@emergensys.net.
