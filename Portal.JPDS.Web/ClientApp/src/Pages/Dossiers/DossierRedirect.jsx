import { CircularProgress } from '@material-ui/core';
import React, { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux';
import { useParams, useHistory } from 'react-router-dom';
import { getDossierGeneralInfo } from '../../apis/dossiersApi';
import { commonActions } from '../../_actions';

const DossierRedirect = () => {
    const params = useParams();
    const history = useHistory()
    const [isLoading, setIsLoading] = useState(false);
    const [isChangeProject, setIsChangeProject] = useState(false);
    const { loading, all, selected } = useSelector(state => state.buildings);
    const dispatch = useDispatch();

    useEffect(() => {
        const { dossierId } = params;
        if (all.length) {
            setIsLoading(true);
            getDossierGeneralInfo(dossierId).then(({ data }) => {
                const projectId = data.projectId;
                if (selected && selected.projectId === projectId)
                    history.push(`/werk/${selected.projectNo}${history.location.pathname + history.location.search}`)
                else {
                    const selectedProjects = all.filter(x => x.projectId === projectId);
                    if (selectedProjects && selectedProjects.length > 0) {
                        dispatch(commonActions.selectBuilding(selectedProjects[0]));
                        setIsChangeProject(true);
                        return;
                    }
                }
                setIsLoading(false);

            }).catch(er => {
                setIsLoading(false);
            })
        }
    }, [all]);

    useEffect(() => {
        if (selected && isChangeProject) {
            setIsLoading(false);
            history.push(`/werk/${selected.projectNo}${history.location.pathname + history.location.search}`)
        }
    }, [selected])

    return isLoading || loading ? (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            <CircularProgress size={55} color={'primary'} />
        </div>
    ) : null
}
export default DossierRedirect;