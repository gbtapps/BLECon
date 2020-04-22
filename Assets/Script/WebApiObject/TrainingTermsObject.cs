using System;
using System.Collections.Generic;

public class Young
{
    public int min { get; set; }
    public int max { get; set; }
}

public class Middle
{
    public int min { get; set; }
    public int max { get; set; }
}

public class Senior
{
    public int min { get; set; }
    public int max { get; set; }
}

public class Silver
{
    public int min { get; set; }
    public int max { get; set; }
}

public class ScoringSetting
{
    public Young young { get; set; }
    public Middle middle { get; set; }
    public Senior senior { get; set; }
    public Silver silver { get; set; }
}

public class Category
{
    public string name { get; set; }
    public string uid { get; set; }
    public List<string> courses { get; set; }
    public string _id { get; set; }
    //public DateTime createdAt { get; set; }
    //public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public string id { get; set; }
    public object trainings { get; set; }
}

//  trainingTermsで返ってくるほう
public class Training
{
    public string name { get; set; }
    public string uid { get; set; }
    public int maxLevel { get; set; }
    public ScoringSetting scoringSetting { get; set; }
//    public List<string> trainingterms { get; set; }
    public string _id { get; set; }
//   public DateTime createdAt { get; set; }
//    public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public string id { get; set; }
    public Category category { get; set; }
}
public class TrainingTerms
{
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string _id { get; set; }
    //public DateTime createdAt { get; set; }
    //public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public string id { get; set; }
    public List<Training> trainings { get; set; }
}
