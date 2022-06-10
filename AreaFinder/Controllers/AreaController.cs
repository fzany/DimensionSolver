using AreaFinder.Helpers;
using AreaFinder.Models;
using Microsoft.AspNetCore.Mvc;

namespace AreaFinder.Controllers;

[ApiController]
[Route("[controller]")]
public class AreaController : ControllerBase
{

    private readonly ILogger<AreaController> _logger;

    public AreaController(ILogger<AreaController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Program to derive an Area from a given Area and Aspect Ratio.
    /// </summary>
    /// <param name="ratioX"></param>
    /// <param name="ratioY"></param>
    /// <param name="Lenght"></param>
    /// <param name="Breath"></param>
    /// <returns></returns>
    [HttpGet("GetArea/{ratioFrom},{ratioTo},{Lenght},{Breath}")]
    public ActionResult<(string, string)> Get(int ratioFrom, int ratioTo, int Lenght, int Breath)
    {

        #region Ratio
        var ratioResult = Calculator.GetLowestDivisor(ratioFrom, ratioTo);

        var ratioX = ratioResult.Item1;
        var ratioY = ratioResult.Item2;


        #endregion

        #region Dimension
        var dimensionResult = Calculator.GetLowestDivisor(Lenght, Breath);

        var dimensionX = dimensionResult.Item1;
        var dimensionY = dimensionResult.Item2;
        var dimensionMultiplier = dimensionResult.Item3;
        Console.WriteLine(dimensionResult);

        var lowerDimension = Math.Min(dimensionX, dimensionY);
        var higherDimension = Math.Max(dimensionX, dimensionY);




        //Create Dimentoon
        List<Dual> duoDimension = new List<Dual>()
{
    new Dual() { Position = Position.X, Value = dimensionX, Type = lowerDimension == dimensionX ? Dimension.Lower : Dimension.Higher },
    new Dual() { Position = Position.Y, Value = dimensionY, Type = lowerDimension == dimensionY ? Dimension.Lower : Dimension.Higher }
};


        //Create Ratio
        var lowerRatio = Math.Min(ratioX, ratioY);
        var higherRatio = Math.Max(ratioX, ratioY);

        if (lowerRatio == higherRatio)
        {
            var _ = (ratioX * dimensionMultiplier, ratioY * dimensionMultiplier);
            return Ok(_);
        }

        //Check if the dimention rhymes

        if (lowerRatio > lowerDimension || lowerRatio > higherDimension || higherRatio > lowerDimension || higherRatio > higherDimension)
        {
            var __ = (dimensionX * dimensionMultiplier, dimensionY * dimensionMultiplier, dimensionMultiplier);
            return Ok(__);
        }

        List<Dual> duoRatio = new List<Dual>() {
    new Dual() { Position = Position.X, Value = ratioX, OriginalValue = ratioX, FinalValue = ratioX, Type = ratioX == lowerRatio ? Dimension.Lower : Dimension.Higher },
     new Dual() { Position = Position.Y, Value = ratioY, OriginalValue = ratioY, FinalValue = ratioY, Type = ratioY == lowerRatio ? Dimension.Lower : Dimension.Higher }
};

        double ratioIncrementor = (double)lowerRatio / (double)higherRatio;
        bool ContinueLoop = true;
        while (ContinueLoop)
        {
            //increase the lower ratio by 1.
            duoRatio.FirstOrDefault(d => d.Type == Dimension.Lower).Value += ratioIncrementor;

            //Increase the higher ration by the multiply of the high
            duoRatio.FirstOrDefault(d => d.Type == Dimension.Higher).Value = duoRatio.FirstOrDefault(d => d.Type == Dimension.Lower).Value * duoRatio.FirstOrDefault(d => d.Type == Dimension.Higher).OriginalValue;




            ContinueLoop = duoRatio.FirstOrDefault(d => d.Position == Position.X).Value <= lowerDimension &&
                            duoRatio.FirstOrDefault(d => d.Position == Position.X).Value <= higherDimension &&
                            duoRatio.FirstOrDefault(d => d.Position == Position.Y).Value <= lowerDimension &&
                            duoRatio.FirstOrDefault(d => d.Position == Position.Y).Value <= higherDimension

                            ;

            if (ContinueLoop)
            {
                duoRatio.FirstOrDefault(d => d.Position == Position.X).FinalValue = duoRatio.FirstOrDefault(d => d.Position == Position.X).Value;
                duoRatio.FirstOrDefault(d => d.Position == Position.Y).FinalValue = duoRatio.FirstOrDefault(d => d.Position == Position.Y).Value;

            }
            Console.WriteLine((duoRatio.FirstOrDefault(d => d.Position == Position.X).FinalValue * dimensionMultiplier, duoRatio.FirstOrDefault(d => d.Position == Position.Y).FinalValue * dimensionMultiplier, dimensionMultiplier));

        }

        //Multiply the current ratios by the number times it was reduced.
        return Ok($"New Area is {(duoRatio.FirstOrDefault(d => d.Position == Position.X).FinalValue * dimensionMultiplier)} x {(duoRatio.FirstOrDefault(d => d.Position == Position.Y).FinalValue * dimensionMultiplier)}");


        #endregion
    }
}

