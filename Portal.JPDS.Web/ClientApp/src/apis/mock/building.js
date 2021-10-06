import { URLS } from "../urls";
import { mock } from "./mock";

const buildings = [
    {
        dossierId: "017F7435-60C5-4DF2-9B45-A83253CE1A0A",
        projectId: "0077",
        name: "Test Dossier Name",
        generalInformation: "Test Name",
        backgroundImage: '"https://dev.huisinfo.nl/api/Config/WebBackground"',
        deadline: "2021-04-25T15:36:09.5700000+02:00",
        status: 0,
        isArchived: false,
        createdBy: "Willem de Vries",
        closedOn: null,
        isGeneralFilesSectionAvailable: true,
        isObjectFilesSectionAvailable: true,
        buildingIds: [
            {
                buildingId: "11B97E9F-6EFA-4535-97B6-60C1BE2AA461",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
                internalObjectImages: [],
                externalObjectImages: []

            }
        ],
        userList: null,
        internalImages: [],
        externalImages: [],
        isOverdue: false,
        is48hoursReminder: false

    }
];

mock.onGet(URLS.GET_DOSSIER_BUILDING_INFO).reply(200, {
    data: buildings,
    message: 'Get Dossier Building Successfully.',
    status: true
});