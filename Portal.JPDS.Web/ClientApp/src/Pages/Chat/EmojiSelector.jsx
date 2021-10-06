import React, { useState, useEffect } from "react";
import { IconButton, Menu, MenuItem, makeStyles, Popover, Fab } from "@material-ui/core";
import { SentimentSatisfiedAlt } from "@material-ui/icons";
import { emojis } from "../../_constants";

const useStyles = makeStyles((theme) => ({
    emojiContainer: {
        width: '100%',
        maxWidth: 290
    },
    emojiButton: {
        width: 58,
        color: 'initial'
    }
}));

export default function EmojiSelector(props) {
    const { disabled, onSelect, ...rest } = props;
    const classes = useStyles();
    const [anchorEl, setAnchorEl] = React.useState(null);
    const [color, setColor] = React.useState("default");

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
        setColor("primary");
    };

    const handleClose = () => {
        setAnchorEl(null);
        setColor("default");
    };

    const onClickEmoji = (value) => {
        onSelect(value);
        handleClose();
    }

    const open = Boolean(anchorEl);
    const id = open ? 'emoji-popover' : undefined;

    return (
        <React.Fragment>
            <IconButton aria-describedby={id} disabled={disabled} onClick={handleClick} color={color} {...rest}>
                <SentimentSatisfiedAlt />
            </IconButton>
            <Popover
                id={id}
                open={open && disabled!==true}
                anchorEl={anchorEl}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: 'top',
                    horizontal: 'center',
                }}
                transformOrigin={{
                    vertical: 'bottom',
                    horizontal: 'center',
                }}
            >
                <div className={classes.emojiContainer}>
                    {
                        emojis.map((emoji, index) => (
                            <IconButton className={classes.emojiButton} key={index} onClick={() => onClickEmoji(emoji.value)}>
                                {emoji.content}
                            </IconButton>
                        ))
                    }
                </div>
            </Popover>
        </React.Fragment>
    );
}