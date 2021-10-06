import { Tab } from '@material-ui/core';
import React, { useEffect, useRef } from 'react';
import { useDrop } from 'react-dnd';
import { useTheme } from '@material-ui/core/styles';

let timeOutId;
export const TabButton = ({ tab, setActiveTab, activeTab, type, ...props }) => {
    const ref = useRef(null);
    const theme = useTheme();

    const [{ isOver, canDrop }, drop] = useDrop({
        accept: "moveAttachments",
        drop: () => ({ name: tab, type }),
        collect: (monitor) => ({
            isOver: monitor.isOver(),
            canDrop: monitor.canDrop()
        }),
        canDrop() { return false },
        hover(dragItem, monitor) {
            if (!ref.current) return;
        }
    });

    useEffect(() => {
        if (tab !== activeTab && isOver && !canDrop) {
            clearTimeout(timeOutId);
            timeOutId = setTimeout(function () { setActiveTab(tab); }, 600);
        } else if (!isOver && tab !== activeTab) clearTimeout(timeOutId);
    }, [isOver, canDrop])

    const getBackground = () => {
        if (isOver) {
            if (tab === activeTab) {
                return theme.palette.error.light;
            } else if (!canDrop) {
                return theme.palette.info.light;
            }
        }
        return "";
    };

    return <Tab ref={drop} {...props} style={{ background: getBackground() }} />
}
export default TabButton;