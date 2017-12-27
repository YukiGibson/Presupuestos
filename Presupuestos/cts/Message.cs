using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.ViewModels;
using System.Text; 

namespace Presupuestos.cts
{
    //public abstract class Message
    //{
    //    protected Dictionary<string, string> message;
    //    protected List<MonthViewModel> insertedMonth;
    //    protected List<ProjectionViewModel> Projections;
    //    protected StringBuilder content;


    //    public Message(Dictionary<string, string> Message, List<MonthViewModel> Month, List<ProjectionViewModel> Projection)
    //    {
    //        this.message = Message;
    //        this.insertedMonth = Month;
    //        this.Projections = Projection;
    //        this.content = new StringBuilder();
    //    } // End ctor

    //    public void DialogMessage()
    //    {
    //        foreach (var item in insertedMonth.Where(p => p.value != "0"))
    //        {
    //            content.Append(String.Format("- {0} del mes {1} ", item.value, item.month));
    //        }
    //        content.Append(" a los presupuestos ");
    //        foreach (var item in Projections.Where(p => p.Checked))
    //        {
    //            content.Append(String.Format("{0}, ", item.Presupuesto));
    //        }
    //    }
    //}

    //public class DashBoardMessage : Message
    //{
    //    public DashBoardMessage(Dictionary<string, string> Message, List<MonthViewModel> Month, List<ProjectionViewModel> Projection) : base(Message, Month, Projection) { }

    //    public Dictionary<string, string> BuildMessage(bool success)
    //    {
    //        if (success)
    //        {
    //            if (isChecked())
    //            {
    //                content.Append("Se ingresaron los valores ");
    //                DialogMessage();
    //                content.Append("de forma exitosa");
    //                message.Add("Success", content.ToString());
    //            }
    //            else
    //            {
    //                content.Append("Ningún presupuesto fue escogido");
    //                message.Add("Alert", content.ToString());
    //            }
    //        }
    //        else
    //        {
    //            content.Append("Se falló el ingreso los valores");
    //            //DialogMessage(); Lanza error ya que si no hay presupuestos, la Lista Genérica está null
    //            content.Append(" de forma exitosa");
    //            message.Add("Error", content.ToString());
    //        }
    //        return message;
    //    }

    //    private bool isChecked()
    //    {
    //        return Projections.Where(p => p.Checked).Any();
    //    }
    //}
}