import React, { useState, useEffect } from "react";
import MUIRichTextEditor from "mui-rte";
import { convertFromRaw, convertToRaw, EditorState } from 'draft-js'
import { mdToDraftjs } from 'draftjs-md-converter';
import { ThemeProvider } from '@material-ui/styles';
import { useTheme } from '@material-ui/core/styles';

const local_theme_overrides = {
    overrides: {
        MUIRichTextEditor: {
            root: {
                //backgroundColor: "#ebebeb",
                //width: "90%"
            },
            container: {
                position: 'inherit',
                fontFamily: 'inherit',
                margin: 0
            },
            editor: {
                //overflow: "auto"
            },
            editorContainer: {
                cursor: 'inherit',
                margin: 0,
                padding: 0,
                "& *": {
                    maxWidth: "100%"
                }
            }
        }
    }
};

export default function Markdown(props) {

    const { value } = props;
    const theme = useTheme();
    const [localTheme, setLocalTheme] = useState(theme);
    const rawData = mdToDraftjs(value);
    const newEditorState = EditorState.createWithContent(convertFromRaw(rawData));
    const contentState = JSON.stringify(convertToRaw(newEditorState.getCurrentContent()));
    const [initial, setInitial] = useState(contentState);

    useEffect(() => {
        setLocalTheme(Object.assign({ ...theme }, local_theme_overrides));

        const rawData = mdToDraftjs(value);
        const newEditorState = EditorState.createWithContent(convertFromRaw(rawData));
        const contentState = JSON.stringify(convertToRaw(newEditorState.getCurrentContent()));

        setInitial(contentState)
    }, [value]);


    return (
        <ThemeProvider theme={localTheme}>
            <MUIRichTextEditor
                toolbar={false}
                inheritFontSize
                controls={[]}
                readOnly={true}
                defaultValue={initial}
            />
        </ThemeProvider>
    );
}