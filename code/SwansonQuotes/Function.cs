using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using Alexa.NET.Response;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SwansonQuotes
{
    public class Function
    {
        public List<FactResource> GetResources()
        {
            var resources = new List<FactResource>();
            var factResource = new FactResource("en-US")
            {
                SkillName = "Ron Swanson Quotes",
                GetFactMessage = "Here's your Ron Swanson quote: ",
                HelpMessage = "You can say tell me a Ron Swanson quote, or, you can say exit... What do you want?",
                HelpReprompt = "You can say tell me a Ron Swanson quote to start",
                StopMessage = "Bye!"
            };

            factResource.Facts.Add("Never half-ass two things. Whole-ass one thing.");
            factResource.Facts.Add("Clear alcohols are for rich women on diets.");
            factResource.Facts.Add("Dear frozen yogurt, you are the celery of desserts. Be ice cream, or be nothing.");
            factResource.Facts.Add("Fishing relaxes me. It's like yoga, except I get to kill something.");
            factResource.Facts.Add("There's only one thing I hate more than lying: skim milk. Which is water that's "
                                    + "lying about being milk.");
            factResource.Facts.Add("You had me at meat tornado.");
            factResource.Facts.Add("I've never known what bangs are, and I don't intend to learn.");
            factResource.Facts.Add("Any dog under fifty pounds is a cat, and cats are useless.");
            factResource.Facts.Add("Put some alcohol in your mouth to keep the words from coming out.");
            factResource.Facts.Add("Crying: acceptable at funerals and the Grand Canyon.");
            factResource.Facts.Add("When I eat, it is the food that is scared.");
            factResource.Facts.Add("History began July 4th, 1776. Anything before that was a mistake.");
            factResource.Facts.Add("Give a man a fish and feed him for a day. Don’t teach a man to fish, and feed yourself. " +
                                    "He’s a grown man. And fishing’s not that hard.");
            factResource.Facts.Add("Great job, everyone. The reception will be held in each of our individual houses, alone.");
            factResource.Facts.Add("When people get a little too chummy with me I like to call them by the wrong name " +
                                    "to let them know I don’t really care about them.");
            factResource.Facts.Add("Friends: one to three is sufficient.");
            factResource.Facts.Add("There is only one bad word: taxes.");
            factResource.Facts.Add("It’s always a good idea to demonstrate to your coworkers that you are capable " +
                                    "of withstanding a tremendous amount of pain.");
            factResource.Facts.Add("I once worked with a guy for three years and never learned his name. Best friend " +
                                    "I ever had. We still never talk sometimes.");

            resources.Add(factResource);
            return resources;
        }

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var response = new SkillResponse { Response = new ResponseBody { ShouldEndSession = false } };
            IOutputSpeech innerResponse = null;

            var allResources = GetResources();
            var resource = allResources.FirstOrDefault();

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                innerResponse = new PlainTextOutputSpeech();
                ((PlainTextOutputSpeech)innerResponse).Text = EmitNewFact(resource, true);

            }

            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;

                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        innerResponse = new PlainTextOutputSpeech();
                        ((PlainTextOutputSpeech)innerResponse).Text = resource.StopMessage;
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.StopIntent":
                        innerResponse = new PlainTextOutputSpeech();
                        ((PlainTextOutputSpeech)innerResponse).Text = resource.StopMessage;
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        innerResponse = new PlainTextOutputSpeech();
                        ((PlainTextOutputSpeech)innerResponse).Text = resource.HelpMessage;
                        break;
                    case "GetFactIntent":
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = EmitNewFact(resource, false);
                        break;
                    case "GetNewFactIntent":
                        innerResponse = new PlainTextOutputSpeech();
                        ((PlainTextOutputSpeech)innerResponse).Text = EmitNewFact(resource, false);
                        break;
                    default:
                        innerResponse = new PlainTextOutputSpeech();
                        ((PlainTextOutputSpeech)innerResponse).Text = resource.HelpReprompt;
                        break;
                }
            }

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            return response;
        }
        public string EmitNewFact(FactResource resource, bool doIncludePreface)
        {
            var randomIterator = new Random();

            if (doIncludePreface)
            {
                return resource.GetFactMessage + resource.Facts[randomIterator.Next(resource.Facts.Count)];
            }

            return resource.Facts[randomIterator.Next(resource.Facts.Count)];
        }
    }
}
