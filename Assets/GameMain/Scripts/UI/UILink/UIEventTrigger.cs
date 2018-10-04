using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIEventTrigger : EventTrigger
{
    public static UIEventTrigger current;

    public List<EventDelegate> onHoverOver = new List<EventDelegate>();

    public List<EventDelegate> onHoverOut = new List<EventDelegate>();

    public List<EventDelegate> onPress = new List<EventDelegate>();

    public List<EventDelegate> onRelease = new List<EventDelegate>();

    public List<EventDelegate> onSelect = new List<EventDelegate>();

    public List<EventDelegate> onDeselect = new List<EventDelegate>();

    public List<EventDelegate> onClick = new List<EventDelegate>();

    public List<EventDelegate> onDoubleClick = new List<EventDelegate>();

    public List<EventDelegate> onDragStart = new List<EventDelegate>();

    public List<EventDelegate> onDragEnd = new List<EventDelegate>();

    public List<EventDelegate> onDragOver = new List<EventDelegate>();

    public List<EventDelegate> onDragOut = new List<EventDelegate>();

    public List<EventDelegate> onDrag = new List<EventDelegate>();

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onDragStart, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnCancel(BaseEventData eventData)
    {
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onDrag, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onDragOver, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onDragOut, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
    }

    public override void OnMove(AxisEventData eventData)
    {
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            if (eventData.clickCount == 2)
            {
                EventDelegate.Execute(this.onDoubleClick, new object[]
                {
                    eventData
                });
            }
            else
            {
                EventDelegate.Execute(this.onClick, new object[]
                {
                    eventData
                });
            }
            UIEventTrigger.current = null;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onPress, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onRelease, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onHoverOver, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onHoverOut, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnScroll(PointerEventData eventData)
    {
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onSelect, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (!(UIEventTrigger.current != null))
        {
            UIEventTrigger.current = this;
            EventDelegate.Execute(this.onDeselect, new object[]
            {
                eventData
            });
            UIEventTrigger.current = null;
        }
    }

    public override void OnSubmit(BaseEventData eventData)
    {
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
    }

    private void OnDestroy()
    {
        this.DestroyEvents(this.onHoverOver);
        this.DestroyEvents(this.onPress);
        this.DestroyEvents(this.onRelease);
        this.DestroyEvents(this.onSelect);
        this.DestroyEvents(this.onDeselect);
        this.DestroyEvents(this.onClick);
        this.DestroyEvents(this.onDoubleClick);
        this.DestroyEvents(this.onDragStart);
        this.DestroyEvents(this.onDragEnd);
        this.DestroyEvents(this.onDragOver);
        this.DestroyEvents(this.onDragOut);
        this.DestroyEvents(this.onDrag);
    }

    private void DestroyEvents(List<EventDelegate> Events)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            EventDelegate eventDelegate = Events[i];
            if (eventDelegate != null)
            {
                eventDelegate.Clear();
            }
        }
        Events.Clear();
        Events = null;
    }
}
