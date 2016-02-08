﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TemplateBuilder.Helpers;
using TemplateBuilder.Model;

namespace TemplateBuilder.ViewModel.MainWindow
{
    public partial class TemplateBuilderViewModel
    {
        public class WaitLocation : Templating
        {
            public WaitLocation(TemplateBuilderViewModel outer) : base(outer)
            { }

            public override void PositionInput(Point pos, MouseButton changedButton)
            {
                // The user is starting to record a new minutia

                // Start a new minutia data record.
                MinutiaRecord record = new MinutiaRecord();

                // Save the position TO SCALE.
                record.Location = pos.InvScale(Outer.Scale);
                // Save the current type.
                record.Type = Outer.InputMinutiaType;
                // Record minutia information.
                Outer.Minutae.Add(record);

                // Indicate next input defines the direction.
                TransitionTo(typeof(WaitDirection));
            }

            public override void PositionMove(Point e)
            {
                // Ignore.
            }

            public override void RemoveMinutia(int index)
            {
                // Remove the item at the specified index.
                Outer.Minutae.RemoveAt(index);
            }

            public override void SaveTemplate()
            {
                IntegrityCheck.IsNotNull(Outer.Image);

                byte[] isoTemplate = TemplateHelper.ToIsoTemplate(Outer.Minutae);

                bool isSaved = Outer.m_DataController.SaveTemplate(isoTemplate);

                if (isSaved)
                {
                    // We've finished with this image, so transition to Idle state.
                    TransitionTo(typeof(Idle));
                }
                else
                {
                    // Failed to save the template successfully.
                    // TODO: show dialog to try again?
                    TransitionTo(typeof(Idle));
                }
            }
            public override void EscapeAction()
            {
                // Nothing to escape.
            }

            public override void SetMinutiaType(MinutiaType type)
            {
                // Ignore. No current record to update.
            }

            public override void StartMove(int index)
            {
                Outer.m_SelectedMinutia = index;
                TransitionTo(typeof(MovingMinutia));
            }

            #region Helper Methods

            private static string ToRecord(MinutiaRecord labels)
            {
                return String.Format("{0}, {1}, {2}, {3}",
                    labels.Location.X, labels.Location.Y, labels.Direction, labels.Type);
            }

            #endregion
        }
    }
}