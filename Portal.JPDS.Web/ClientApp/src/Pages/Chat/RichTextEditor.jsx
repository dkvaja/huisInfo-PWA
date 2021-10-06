import React, { useState, useEffect, useRef } from "react";
import MUIRichTextEditor from "mui-rte";
import { convertToRaw, convertFromRaw, EditorState, Entity, Modifier, CharacterMetadata } from 'draft-js';
import { mdToDraftjs, draftjsToMd } from 'draftjs-md-converter';
import { ThemeProvider } from '@material-ui/styles';
import { useTheme } from '@material-ui/core/styles';
import { TextFormat } from "@material-ui/icons";
import { IconButton } from "@material-ui/core";
import Markdown from "../../components/Markdown";
import { stateToHTML } from 'draft-js-export-html';

export default function RichTextEditor(props) {

    const { readOnly, onChange, defaultValue, value, showToolbar, textToInsertAtCursor, richTextToInsertAtCursor, onCompleteTextInsert, ...rest } = props;
    const rteRef = useRef(null);
    const [editorState, setEditorState] = useState(EditorState.createEmpty());
    const [standardText, setStandardText] = useState('');
    // const [currentValue, setCurrentValue] = useState('');
    const [injectEditorState, setInjectEditorState] = useState(false);
    const [replaceAtomic, setReplaceAtomic] = useState('');
    const [initialValue, setInitialValue] = useState(defaultValue);

    // console.debug("CurrentContent", draftjsToMd(convertToRaw(editorState.getCurrentContent())))
    // console.log("richtextAtC", richTextToInsertAtCursor)
    // console.log("dv:", defaultValue)

    const theme = useTheme();
    const [localTheme, setLocalTheme] = useState(theme);

    const minHeightEditor = showToolbar === true ? 160 : 20;

    const local_theme_overrides = {
        overrides: {
            MUIRichTextEditor: {
                root: {
                    //backgroundColor: "#ebebeb",
                    //width: "90%"
                },
                container: {
                    fontFamily: 'inherit',
                    margin: 0,
                    //position: 'relative',
                    '& [class*="Autocomplete-container"]': {
                        //position: 'fixed'
                        maxHeight: 'calc(100% + 16px)',
                        overflow: 'auto'
                    }
                },
                editor: {
                    //backgroundColor: "#ebebeb",
                    //padding: "20px",
                    minHeight: 20,
                    height: 'inherit'
                },
                editorContainer: {
                    margin: 0,
                    minHeight: minHeightEditor,
                    maxHeight: "40vh",
                    overflow: "auto",
                    padding: 8,
                    borderRadius: 4,
                    "& *": {
                        maxWidth: "100%"
                    }
                },
                placeHolder: {
                    position: 'relative',
                    marginBottom: -20
                },
                toolbar: {
                    //margin: '-8px 0 0',
                    whiteSpace: 'nowrap',
                    overflow: 'auto',
                    display: showToolbar === true ? '' : 'none',
                    //borderTop: "1px solid gray",
                    //backgroundColor: "#ebebeb"
                    "& > button": {
                        marginRight: theme.spacing(1)
                    },
                    "& > button:last-child": {
                        marginRight: theme.spacing(0)
                    }
                }
            }
        }
    };

    useEffect(() => {
        setLocalTheme(Object.assign({ ...theme }, local_theme_overrides));
        rteRef.current.focus();
    }, [showToolbar]);

    useEffect(() => {
        console.debug("injectEditorState:", injectEditorState)
        //     if (injectEditorState) onChangeText(editorState);
    }, [injectEditorState])

    useEffect(() => {
        // if (initialValue !== '') {
        console.log('inside useEffect', standardText)
        let rawData = mdToDraftjs(standardText);
        //to update the default value provided initially. it was resseting to empty again and again due to atomic replace code.
        if (standardText !== initialValue && initialValue !== '') {
            console.debug("I am taking initialValue", initialValue);
            rawData = mdToDraftjs(initialValue);
            setInitialValue('');
        }
        const newEditorState = EditorState.moveFocusToEnd(EditorState.createWithContent(convertFromRaw(rawData)));

        setEditorState(newEditorState);
        setInjectEditorState(true);
        console.log("contentInsideUseEffectstandardText:", draftjsToMd(convertToRaw(newEditorState.getCurrentContent())));
        rteRef.current.focus();
        // }
    }, [standardText]);

    useEffect(() => {
        // // if (initialValue !== '') {
        // let rawData = mdToDraftjs(value);
        // //to update the default value provided initially. it was resseting to empty again and again due to atomic replace code.
        // if (value !== initialValue && initialValue !== '') {
        //     rawData = mdToDraftjs(initialValue);
        //     setInitialValue('');
        // }
        // const newEditorState = EditorState.moveFocusToEnd(EditorState.createWithContent(convertFromRaw(rawData)));

        // setEditorState(newEditorState);
        // console.log("contentInsideUseEffectDefaultValue:", draftjsToMd(convertToRaw(newEditorState.getCurrentContent())));
        // setInjectEditorState(true);
        // rteRef.current.focus();
        // // }
    }, [value]);

    useEffect(() => {
        // if (initialValue !== '') {
        let rawData = mdToDraftjs(defaultValue);
        //to update the default value provided initially. it was resseting to empty again and again due to atomic replace code.
        if (defaultValue !== initialValue && initialValue !== '') {
            rawData = mdToDraftjs(initialValue);
            setInitialValue('');
        }
        const newEditorState = EditorState.moveFocusToEnd(EditorState.createWithContent(convertFromRaw(rawData)));

        setEditorState(newEditorState);
        setInjectEditorState(true);
        rteRef.current.focus();
        console.log("contentInsideUseEffectDefaultValue:", draftjsToMd(convertToRaw(newEditorState.getCurrentContent())));
        // }
    }, [defaultValue]);

    useEffect(() => {
        console.log("textToInsertAtCursor:", textToInsertAtCursor);
        if (textToInsertAtCursor && textToInsertAtCursor.trim() !== '')
            addContent(textToInsertAtCursor);
    }, [textToInsertAtCursor]);

    useEffect(() => {
        console.log("richTextToInsertAtCursor:", richTextToInsertAtCursor);
        if (richTextToInsertAtCursor && richTextToInsertAtCursor.trim() !== '')
            addContent(richTextToInsertAtCursor, true);
    }, [richTextToInsertAtCursor]);

    function onChangeText(data) {
        console.debug("i am inside onchange")
        const currentContent = data.getCurrentContent();
        const content = draftjsToMd(convertToRaw(currentContent));
        console.log("content:", content)
        if (content.includes('\n![](undefined)\n')) {
            if (replaceAtomic !== '') {
                const richText = replaceAtomic.slice();
                console.debug("richText:", richText)
                setReplaceAtomic('');
                setStandardText(content.replace('![](undefined)\n', richText));
            }
        }
        else {
            // setCurrentValue(content);
            // if (content !== value)
            setEditorState(data);
            setTimeout(() => onChange(content), 0);
            setInjectEditorState(false);
        }
    }

    function addContent(value, isRichText = false) {
        if (isRichText === false) {
            const es = editorState ? editorState : EditorState.createEmpty();

            const selection = es.getSelection().set('hasFocus', true);
            const contentState = es.getCurrentContent();
            const ncs = Modifier.insertText(contentState, selection, value);
            const final = EditorState.push(es, ncs, 'insert-fragment');

            setEditorState(final);
        }
        else {
            const rawData = mdToDraftjs(value);
            const convertedRawData = convertFromRaw(rawData);
            if (editorState) {
                var contentState = editorState.getCurrentContent();
                var selectionState = editorState.getSelection();

                var afterRemoval = Modifier.removeRange(contentState, selectionState, 'backward');

                var targetSelection = afterRemoval.getSelectionAfter();
                var afterSplit = Modifier.splitBlock(afterRemoval, targetSelection);
                var insertionTarget = afterSplit.getSelectionAfter();

                var asAtomicBlock = Modifier.setBlockType(afterSplit, insertionTarget, 'insert-fragment');

                var withAtomicBlock = Modifier.replaceWithFragment(asAtomicBlock, insertionTarget, convertedRawData.getBlockMap());

                var newContent = withAtomicBlock.merge({
                    selectionBefore: selectionState,
                    selectionAfter: withAtomicBlock.getSelectionAfter().set('hasFocus', true)
                });

                var final = EditorState.push(editorState, newContent, 'insert-fragment')
                setEditorState(final);
                console.log("finalStateValueinRichtextwithnew:", draftjsToMd(convertToRaw(final.getCurrentContent())));
            }
            else {
                const newEditorState = EditorState.createWithContent(convertedRawData);
                console.log("newEditorStateValueinRichtextwithnew:", draftjsToMd(convertToRaw(newEditorState.getCurrentContent())));
                setEditorState(newEditorState);
            }
        }
        onCompleteTextInsert();
        setInjectEditorState(true);
        rteRef.current.focus();
    }

    const MarkDownCustomControl = (props) => {
        const { blockProps } = props
        const { value } = blockProps // Get the value provided in the TAutocompleteItem[]

        //const handleClick = () => {
        //    addContent(value, true)
        //}

        //const rawData = mdToDraftjs(value);
        //const newEditorState = EditorState.createWithContent(convertFromRaw(rawData));
        //let options = {
        //    defaultBlockTag: null,
        //};
        //let html = stateToHTML(newEditorState.getCurrentContent(), options);
        //return <div dangerouslySetInnerHTML={{ __html: html }}></div>

        setReplaceAtomic(value);
        return <></>;
    }


    return (
        <ThemeProvider theme={localTheme}>
            <MUIRichTextEditor
                ref={rteRef}
                inheritFontSize
                toolbarButtonSize="small"
                readOnly={readOnly}
                controls={[
                    "title",
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "undo",
                    "redo",
                    "link",
                    //"media",
                    "numberList",
                    "bulletList",
                    "quote",
                    "code",
                    "clear"
                ]}
                customControls={[
                    {
                        name: "markdown",
                        type: "atomic",
                        atomicComponent: MarkDownCustomControl
                    },
                ]}
                //inlineToolbar={true}
                onChange={onChangeText}
                draftEditorProps={injectEditorState === true && { editorState }}
                {...rest}
            />
            {
                //<IconButton aria-label="Chat" onClick={addContent}>
                //    <TextFormat />
                //</IconButton>
            }
        </ThemeProvider>
    );
}