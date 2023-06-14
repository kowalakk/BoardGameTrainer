using Game.IGame;

namespace BoardGameTrainer
{
    public class StopConditionFrame : DropDownFrame<IStopConditionFactory>
    {
        private SpinButtonBox[] Boxes { get; } = new SpinButtonBox[2];
        private int VisibleBox { get; set; } = 0;
        public int Param { get; set; } = 1000;

        public event EventHandler ParamChanged
        {
            add { Boxes[0].Changed += value; Boxes[1].Changed += value; DropDown.Changed += value; }
            remove { Boxes[0].Changed -= value; Boxes[1].Changed -= value; DropDown.Changed -= value; }
        }

        public StopConditionFrame(Dictionary<string, IStopConditionFactory> entries) : base("Stop condition", entries)
        {

            Boxes[0] = new(1000, 100000, 100, Param, "miliseconds");
            Boxes[0].Changed += (sender, args) => { Param = (int)Boxes[0].Value; };

            Boxes[1] = new(1000, 100000, 100, Param, "iterations");
            Boxes[1].Changed += (sender, args) => { Param = (int)Boxes[1].Value; };

            Box.PackStart(Boxes[0], true, true, 5);
            Box.PackStart(Boxes[1], true, true, 5);

            Boxes[VisibleBox].Show();
            DropDown.Changed += (sender, args) => 
            { 
                Boxes[VisibleBox].Hide(); 
                Boxes[1 - VisibleBox].Show();
                VisibleBox = 1 - VisibleBox;
                Param = (int)Boxes[VisibleBox].Value;
            };
        }
    }
}
