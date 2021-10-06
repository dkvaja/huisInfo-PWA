import React, { useState, useEffect } from "react";
import {
    makeStyles,
    Dialog,
    DialogTitle,
    DialogContent,
    Button,
    Typography,
    Grid,
    Switch,
    FormControlLabel
} from "@material-ui/core";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { CircularProgress } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
    grow: {
        flexGrow: 1
    },
    disabledSwitch: {
        color: `${theme.palette.primary.light} !important`,
    }
}));



export default function EditDossierObjects(props) {
    const { open, buildings, selectedObjects, onSave, onClose, isReadOnly, ...rest } = props;
    const { t } = useTranslation();
    const classes = useStyles();
    const [selectedObjectsDraft, setSelectedObjectsDraft] = useState(selectedObjects);
    const [changed, setChanged] = useState([]);
    const { updateLoading } = useSelector(state => state.dossier);
    const [isUpdated, setIsUpdated] = useState(false)
    useEffect(() => {
        setSelectedObjectsDraft(selectedObjects);
    }, [selectedObjects]);

    useEffect(() => {
        if (updateLoading) setIsUpdated(true);
        if (!updateLoading && isUpdated) onClose()
    }, [updateLoading])

    const handleCloseObjects = () => {
        onClose();
    }

    const handleSaveObjects = () => {
        onSave(selectedObjectsDraft, changed);
    }

    const selectAllObjects = () => {
        setSelectedObjectsDraft(buildings);
    };

    const handleChangeSelectAllObjects = ({ target: { checked } }) => {
        if (checked === true) {
            selectAllObjects();
        }
        else {
            setSelectedObjectsDraft([]);
        }
        setChanged(buildings.map(b => ({ buildingId: b.buildingId, isActive: checked })))
    };

    function renderSelectBuilding(building) {
        const selectedObject = selectedObjectsDraft.find(x => x.buildingId === building.buildingId);
        return (
            <Grid container alignItems="center">
                <Grid item>
                    <FormControlLabel
                        style={{ margin: 0 }}
                        value={building.buildingId}
                        disabled={isReadOnly}
                        control={
                            <Switch
                                color="primary"
                                disabled={isReadOnly}
                                checked={!!selectedObject}
                                classes={{ disabled: !!selectedObject && classes.disabledSwitch }}
                                onChange={
                                    () => {
                                        let allChangedData = Object.assign([], changed);
                                        let object = { ...selectedObject };
                                        if (!!selectedObject) {
                                            object = {
                                                buildingId: object.buildingId,
                                                isActive: false,
                                            }

                                            const objects = selectedObjectsDraft.filter(x => x.buildingId !== building.buildingId).slice();
                                            setSelectedObjectsDraft(objects);
                                        }
                                        else {
                                            let objects = selectedObjectsDraft.slice();
                                            object = {
                                                buildingId: building.buildingId,
                                                isActive: true,
                                            }
                                            objects.push({ buildingId: building.buildingId });
                                            setSelectedObjectsDraft(objects);
                                        }
                                        const isFound = allChangedData.findIndex(f => f.buildingId === object.buildingId);
                                        if (isFound >= 0)
                                            allChangedData[isFound] = { ...allChangedData[isFound], ...object };
                                        else allChangedData.push(object)
                                        setChanged(allChangedData)

                                    }
                                }
                            />
                        }
                        label={building.buildingNoExtern}
                        labelPlacement="start"
                    />

                </Grid>
            </Grid>
        )
    }

    return (
        open === true &&
        <Dialog open={open} onClose={updateLoading ? () => { } : handleCloseObjects} aria-labelledby="form-dialog-title" maxWidth='sm' scroll="paper">
            <DialogTitle id="dialog-objects-title" disableTypography>
                <Grid container spacing={1}>
                    <Grid item className={classes.grow}>
                        <Typography variant="h6">{t('Selecteer objecten')}</Typography>
                    </Grid>
                    <Grid item>
                        <Button
                            disabled={updateLoading}
                            variant="outlined"
                            onClick={handleCloseObjects}
                        >
                            {t('Annuleer')}
                        </Button>
                    </Grid>
                    <Grid item>
                        <Button
                            disabled={updateLoading || !changed.length || isReadOnly}
                            variant="outlined"
                            onClick={handleSaveObjects}
                        >
                            {updateLoading ? <CircularProgress size={20} /> : t('Opslaan')}
                        </Button>
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container alignItems="center">
                            <Grid item>
                                <FormControlLabel
                                    style={{ margin: 0 }}
                                    disabled={isReadOnly}
                                    value="all"
                                    control={
                                        <Switch
                                            color="primary"
                                            classes={{ disabled: selectedObjectsDraft.length === buildings.length && classes.disabledSwitch }}
                                            disabled={isReadOnly}
                                            checked={selectedObjectsDraft.length === buildings.length}
                                            onChange={handleChangeSelectAllObjects}
                                        />
                                    }
                                    label={t('Alle')}
                                    labelPlacement="start"
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </DialogTitle>
            <DialogContent>
                <Grid container spacing={1}>
                    <Grid item xs={12} sm={4}>
                        {
                            buildings.map((building, index) => {
                                return index < buildings.length / 3 && (
                                    <React.Fragment key={index}>
                                        {renderSelectBuilding(building)}
                                    </React.Fragment>
                                )
                            })
                        }
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        {
                            buildings.map((building, index) => {
                                const itemsPerSection = buildings.length / 3;
                                return index >= itemsPerSection && index < (itemsPerSection * 2) && (
                                    <React.Fragment key={index}>
                                        {renderSelectBuilding(building)}
                                    </React.Fragment>
                                )
                            })
                        }
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        {
                            buildings.map((building, index) => {
                                return index >= (buildings.length * 2 / 3) && (
                                    <React.Fragment key={index}>
                                        {renderSelectBuilding(building)}
                                    </React.Fragment>
                                )
                            })
                        }
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}
