import { URLS } from "../urls";
import { mock } from "./mock";
const dossiers = {
    openOrClosedDossiers: [
        {
            id: "017F7435-60C5-4DF2-9B45-A83253CE1A0A",
            name: "Dossier 1",
            deadline: "2021-04-19T07:22:39.198Z",
            status: 1,
            projectId: "0077",
            isVisibleToBuyer: true
        },
        {
            id: "017F7435-60C5-4DF2-9B45-A83253CESKCW",
            projectId: "0077",
            name: "Dossier 2",
            deadline: "2021-04-19T07:22:39.198Z",
            status: 2,
            isVisibleToBuyer: true
        },
    ],
    draftDossiers: [
        {
            id: "017F7435-60C5-4DF2-9B45-A83253CEVKSP",
            projectId: "0077",
            name: "Dossier 4",
            deadline: "2021-04-19T07:22:39.198Z",
            status: 0,
            isVisibleToBuyer: true
        },
    ],
    archiveDossiers: [
        {
            id: "017F7435-60C5-4DF2-9B45-A83253CEAACM",
            projectId: "0077",
            name: "Dossier 3",
            deadline: "2021-04-19T07:22:39.198Z",
            status: 1,
            isVisibleToBuyer: true
        },
        {
            id: "017F7435-60C5-4DF2-9B45-A83253CE2EDV",
            projectId: "0077",
            name: "Dossier 5",
            deadline: "2021-04-19T07:22:39.198Z",
            status: 1,
            isVisibleToBuyer: true
        },
    ]
}

const dossierGeneralInfo = [
    {
        id: "017F7435-60C5-4DF2-9B45-A83253CE1A0A",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test",
        generalInformation: "test",
        hasBackground: true,
        backgroundImage: null,
        deadline: "2021-04-26T16:29:39.9100000+02:00",
        status: 1,
        isArchived: false,
        createdBy: "Ruben Eilander",
        closedOn: null,
        hasGeneralFiles: true,
        hasObjectBoundFiles: true,
        buildingInfoList: [
            {
                buildingId: "B88F5178-1D95-495B-A4F3-E071FC8753F3",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "11448B89-917D-4084-8CA1-00C61C90A986",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "72762586-670B-4BA1-A7DB-8F7ED0B1596B",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "9C2A78D6-9B3D-4585-B70E-189BBEACA28E",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "11B97E9F-6EFA-4535-97B6-60C1BE2AA461",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "21FFA257-02E9-41AF-A87B-2140F0685E70",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "ABB134B4-C06B-4894-BB15-06418555F48B",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
        ],
        userList: [{
            id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
            name: "Abhishek Saini",
            isInternal: true,
            isRelationVisible: false,
            buyerContactInfo: null,
            userContactInfo: {
                loginId: null,
                buyerRenterId: null,
                organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                name: "Bouwbedrijf JPDS B.V.",
                email: "info@jpds.nl",
                telephone: "(079) 711 27 00",
                relationName: "Abhishek Saini",
                relationTelephone: null,
                relationMobile: "",
                relationEmail: "a.saini@jpds.nl",
                relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                relationPersonSex: 0,
                relationFunctionName: "Uitvoerder"
            }
        }],
        internalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".png",
                    id: 'atachment-2',
                    name: 'testimage.png',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "pdf-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".pdf",
                    id: 'atachment-3',
                    name: 'testimage.pdf',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "pdf-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".docx",
                    id: 'atachment-4',
                    name: 'testimage.docx',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        externalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        isOverdue: false,
        is48hoursReminder: false
    },
    {
        id: "017F7435-60C5-4DF2-9B45-A83253CESKCW",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test",
        generalInformation: "test",
        hasBackground: true,
        backgroundImage: null,
        deadline: "2021-04-26T16:29:39.9100000+02:00",
        status: 1,
        isArchived: false,
        createdBy: "Ruben Eilander",
        closedOn: null,
        hasGeneralFiles: true,
        hasObjectBoundFiles: true,
        buildingInfoList: [{
            buildingId: "7AE2A9D7-1F9D-4744-BD57-86D8AD3901E0",
            isVisibleToBuyer: true,
            deadline: null,
            status: 1,
            isActive: false,
        },
        {
            buildingId: "98968285-9BF0-4E42-A899-7AEB7E3AF5B2",
            isVisibleToBuyer: true,
            deadline: null,
            status: 1,
            isActive: false,
        },
        {
            buildingId: "1EE191C5-D8FB-46CD-99F7-F6D40238CD05",
            isVisibleToBuyer: true,
            deadline: null,
            status: 1,
            isActive: false,
        },
        {
            buildingId: "21AD142D-AA82-4BD1-8371-516E296C5DAF",
            isVisibleToBuyer: true,
            deadline: null,
            status: 1,
            isActive: false,
        },
        {
            buildingId: "A3FB5987-2C22-4532-87CF-74B4B8A89DFE",
            isVisibleToBuyer: true,
            deadline: null,
            status: 1,
            isActive: false,
        },
        ],
        userList: [{
            id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
            name: "Abhishek Saini",
            isInternal: true,
            isRelationVisible: false,
            buyerContactInfo: null,
            userContactInfo: {
                loginId: null,
                buyerRenterId: null,
                organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                name: "Bouwbedrijf JPDS B.V.",
                email: "info@jpds.nl",
                telephone: "(079) 711 27 00",
                relationName: "Abhishek Saini",
                relationTelephone: null,
                relationMobile: "",
                relationEmail: "a.saini@jpds.nl",
                relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                relationPersonSex: 0,
                relationFunctionName: "Uitvoerder"
            }
        }, {
            id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
            name: "Abhishek Saini",
            isInternal: true,
            isRelationVisible: false,
            buyerContactInfo: null,
            userContactInfo: {
                loginId: null,
                buyerRenterId: null,
                organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                name: "Bouwbedrijf JPDS B.V.",
                email: "info@jpds.nl",
                telephone: "(079) 711 27 00",
                relationName: "Abhishek Saini",
                relationTelephone: null,
                relationMobile: "",
                relationEmail: "a.saini@jpds.nl",
                relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                relationPersonSex: 0,
                relationFunctionName: "Uitvoerder"
            }
        }],
        internalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        externalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        isOverdue: false,
        is48hoursReminder: false
    },
    {
        id: "017F7435-60C5-4DF2-9B45-A83253CEAACM",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test",
        generalInformation: "test",
        hasBackground: true,
        backgroundImage: null,
        deadline: "2021-04-26T16:29:39.9100000+02:00",
        status: 1,
        isArchived: true,
        createdBy: "Ruben Eilander",
        closedOn: null,
        hasGeneralFiles: true,
        hasObjectBoundFiles: true,
        buildingInfoList: [
            {
                buildingId: "37E9A83D-4733-4E84-9AD1-A7DBD44F18D4",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "363D96E4-5418-4258-A862-6E58F8250117",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "CF248011-38F3-4217-BE38-4E5DB0245F4A",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
        ],
        userList: [
            {
                id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
                name: "Abhishek Saini",
                isInternal: true,
                isRelationVisible: false,
                buyerContactInfo: null,
                userContactInfo: {
                    loginId: null,
                    buyerRenterId: null,
                    organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                    relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                    name: "Bouwbedrijf JPDS B.V.",
                    email: "info@jpds.nl",
                    telephone: "(079) 711 27 00",
                    relationName: "Abhishek Saini",
                    relationTelephone: null,
                    relationMobile: "",
                    relationEmail: "a.saini@jpds.nl",
                    relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                    relationPersonSex: 0,
                    relationFunctionName: "Uitvoerder"
                }
            }
        ],
        internalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        externalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        isOverdue: false,
        is48hoursReminder: false
    },
    {
        id: "017F7435-60C5-4DF2-9B45-A83253CE2EDV",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test",
        generalInformation: "test",
        hasBackground: true,
        backgroundImage: null,
        deadline: "2021-04-26T16:29:39.9100000+02:00",
        status: 1,
        isArchived: true,
        createdBy: "Ruben Eilander",
        closedOn: null,
        hasGeneralFiles: true,
        hasObjectBoundFiles: true,
        buildingInfoList: [
            {
                buildingId: "92CE3B28-05ED-48DD-895E-894E73ADD93E",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "CC082A76-5C5D-4574-B9D1-A2FF7AFDB905",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "B3947D19-D3E7-4EBB-B362-26A8763FFB24",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
        ],
        userList: [
            {
                id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
                name: "Abhishek Saini",
                isInternal: true,
                isRelationVisible: false,
                buyerContactInfo: null,
                userContactInfo: {
                    loginId: null,
                    buyerRenterId: null,
                    organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                    relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                    name: "Bouwbedrijf JPDS B.V.",
                    email: "info@jpds.nl",
                    telephone: "(079) 711 27 00",
                    relationName: "Abhishek Saini",
                    relationTelephone: null,
                    relationMobile: "",
                    relationEmail: "a.saini@jpds.nl",
                    relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                    relationPersonSex: 0,
                    relationFunctionName: "Uitvoerder"
                }
            }
        ],
        internalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        externalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        isOverdue: false,
        is48hoursReminder: false
    },
    {
        id: "017F7435-60C5-4DF2-9B45-A83253CEVKSP",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test",
        generalInformation: "test",
        hasBackground: true,
        backgroundImage: null,
        deadline: "2021-04-26T16:29:39.9100000+02:00",
        status: 1,
        isArchived: false,
        createdBy: "Ruben Eilander",
        closedOn: null,
        hasGeneralFiles: true,
        hasObjectBoundFiles: true,
        buildingInfoList: [
            {
                buildingId: "B27F0483-B6EE-4671-8BDE-F321D2EA70BB",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "246C05A5-75FE-401F-A7BB-63D8DF5C98DE",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "F167F400-FBA7-4028-96BA-B9942B7501E0",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
        ],
        userList: [
            {
                id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
                name: "Abhishek Saini",
                isInternal: true,
                isRelationVisible: false,
                buyerContactInfo: null,
                userContactInfo: {
                    loginId: null,
                    buyerRenterId: null,
                    organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                    relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                    name: "Bouwbedrijf JPDS B.V.",
                    email: "info@jpds.nl",
                    telephone: "(079) 711 27 00",
                    relationName: "Abhishek Saini",
                    relationTelephone: null,
                    relationMobile: "",
                    relationEmail: "a.saini@jpds.nl",
                    relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                    relationPersonSex: 0,
                    relationFunctionName: "Uitvoerder"
                }
            }
        ],
        internalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        externalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        isOverdue: false,
        is48hoursReminder: false
    },
    {
        id: "262F7EC4-ECB0-4279-9D58-371E1DD8DF4C",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test",
        generalInformation: "test",
        hasBackground: true,
        backgroundImage: null,
        deadline: "2021-04-26T16:29:39.9100000+02:00",
        status: 1,
        isArchived: false,
        createdBy: "Ruben Eilander",
        closedOn: null,
        hasGeneralFiles: true,
        hasObjectBoundFiles: true,
        buildingInfoList: [
            {
                buildingId: "B27F0483-B6EE-4671-8BDE-F321D2EA70BB",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "246C05A5-75FE-401F-A7BB-63D8DF5C98DE",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
            {
                buildingId: "F167F400-FBA7-4028-96BA-B9942B7501E0",
                isVisibleToBuyer: true,
                deadline: null,
                status: 1,
                isActive: false,
            },
        ],
        userList: [
            {
                id: "813E51FF-F9E4-4AAD-A5B0-A9BDE593D562",
                name: "Abhishek Saini",
                isInternal: true,
                isRelationVisible: false,
                buyerContactInfo: null,
                userContactInfo: {
                    loginId: null,
                    buyerRenterId: null,
                    organisatonId: "B287E81D-6D0D-4415-819F-E98879B568D5",
                    relationId: "AB191B42-5FC2-42E4-9BC3-AF6BEF1B40B3",
                    name: "Bouwbedrijf JPDS B.V.",
                    email: "info@jpds.nl",
                    telephone: "(079) 711 27 00",
                    relationName: "Abhishek Saini",
                    relationTelephone: null,
                    relationMobile: "",
                    relationEmail: "a.saini@jpds.nl",
                    relationPersonId: "C4707073-7D0C-4C38-93FE-700727AF264D",
                    relationPersonSex: 0,
                    relationFunctionName: "Uitvoerder"
                }
            }
        ],
        internalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        externalFiles: {
            uploadedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-5",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            archivedFiles: [
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-4",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ],
            deletedFiles: [
                {
                    fileId: "image-1",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-2",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
                {
                    fileId: "image-3",
                    lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                    image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                    mimeType: ".jpg",
                    id: 'atachment-1',
                    name: 'testimage.jpg',
                    uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                    uploadedBy: 'test',
                    isOwner: false
                },
            ]
        },
        isOverdue: false,
        is48hoursReminder: false
    },
]


const roles = [

    {
        roleName: "Onderaannemer",
        usersList: [
            {
                id: "017F7435-60C5-4DF2-9B45-A83253CE1A0A",
                name: "Afdeling Nazorg & Service",
                isInternal: false,
                isRelationVisible: true,
                buyerContactInfo: null,
                userContactInfo: null

            },
            {
                id: "D3209281-AA3A-4383-BD68-9E1462F75BC2",
                name: "Ruben Eilander",
                isInternal: true,
                isRelationVisible: true,
                buyerContactInfo: null,
                userContactInfo: null

            },
        ]
    },
    {
        roleName: "Nazorg medewerker",
        usersList: [
            {
                id: "D3209281-AA3A-4383-BD68-9E1462F75BC2",
                name: "Ruben Eilander",
                isInternal: false,
                isRelationVisible: true,
                buyerContactInfo: null,
                userContactInfo: null

            }
        ]
    },
    {
        roleName: "Uitvoerder",
        usersList: [
            {
                id: "7BFB9455-66B3-4896-8078-C868B0711475",
                name: "Johan van Eijsden",
                isInternal: false,
                isRelationVisible: true,
                buyerContactInfo: null,
                userContactInfo: null
            }
        ]
    },
    {
        roleName: "Kopers-/Huurders-/Klantbegeleider",
        usersList: [
            {
                id: "D3209281-AA3A-4383-BD68-9E1462F75BC2",
                name: "Ruben Eilander",
                isInternal: true,
                isRelationVisible: true,
                buyerContactInfo: null,
                userContactInfo: null
            },
            {
                id: "017F7435-60C5-4DF2-9B45-A83253CE1A0A",
                name: "Afdeling Nazorg & Service",
                isInternal: true,
                isRelationVisible: true,
                buyerContactInfo: null,
                userContactInfo: null
            },
        ]
    }
];


const dossierBuildingInfo = [
    {
        id: "017F7435-60C5-4DF2-9B45-A83253CE1A0A",
        projectId: "AC6BF7C0-0539-4BA9-B431-2E84B3310382",
        name: "Test Dossier Name",
        generalInformation: "Test Name",
        backgroundImage: null,
        deadline: "2021-04-25T15:36:09.5700000+02:00",
        status: 1,
        isArchived: false,
        createdBy: "Willem de Vries",
        closedOn: null,
        isGeneralFilesSectionAvailable: true,
        isObjectFilesSectionAvailable: true,
        buildingInfoList: [
            {
                buildingId: "B88F5178-1D95-495B-A4F3-E071FC8753F3",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
            {
                buildingId: "11448B89-917D-4084-8CA1-00C61C90A986",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
            {
                buildingId: "72762586-670B-4BA1-A7DB-8F7ED0B1596B",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
            {
                buildingId: "9C2A78D6-9B3D-4585-B70E-189BBEACA28E",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
            {
                buildingId: "11B97E9F-6EFA-4535-97B6-60C1BE2AA461",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
            {
                buildingId: "21FFA257-02E9-41AF-A87B-2140F0685E70",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
            {
                buildingId: "ABB134B4-C06B-4894-BB15-06418555F48B",
                isVisibleToBuyer: true,
                deadline: "2021-04-22T16:29:39.9100000+02:00",
                status: 0,
                isActive: true,
                internalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                externalObjectFiles: {
                    uploadedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    archivedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ],
                    deletedFiles: [
                        {
                            fileId: "image-1",
                            lastModifiedOn: "2021-04-22T13:41:44.1900000+02:00",
                            image: "https://dev.huisinfo.nl/api/Config/WebBackground",
                            mimeType: ".jpg",
                            id: 'atachment-1',
                            name: 'testimage.jpg',
                            uploadedAt: '2021-04-25T15:36:09.5700000+02:00',
                            uploadedBy: 'test',
                            isOwner: false
                        },
                    ]
                },
                isOverdue: false,
                is48hoursReminder: false
            },
        ],
        userList: null,
        internalImages: [],
        externalImages: [],
        isOverdue: false,
        is48hoursReminder: false

    },
]



mock.onPost(URLS.GET_DOSSIER_GENERAL_INFO).reply((config) => {
    const { id } = JSON.parse(config.data);
    const data = dossierGeneralInfo.find(p => p.id === id) || {};
    return [200, data]
});

mock.onGet(URLS.GET_ALL_DOSSIERS_BY_PROJECT_ID + 'AC6BF7C0-0539-4BA9-B431-2E84B3310382').reply(200, dossiers);

mock.onPost(URLS.SEND_DOSSIER_NOTIFICATION).reply((config) => {
    return [200, { status: 'success', message: 'sending successfully' }]
});

mock.onDelete(URLS.DELETE_DOSSIER).reply((config) => {
    const { id } = JSON.parse(config.data);
    const draftDossiers = dossiers.draftDossiers.filter(d => d.id !== id);
    dossiers.draftDossiers = draftDossiers;
    return [200, { id, status: 'deleted successfully' }]
});

mock.onGet(URLS.GET_AVAILABLE_USERS_ROLE_BY_PROJECT_ID + 'AC6BF7C0-0539-4BA9-B431-2E84B3310382').reply(200, roles);

mock.onPost(URLS.UPDATE_DOSSIER_RIGHTS).reply((config) => {

    return [200, roles];
});

mock.onPost(URLS.GET_DOSSIER_BUILDING_INFO).reply((config) => {
    const { buildingId, dossierId } = JSON.parse(config.data);
    const dossier = dossierBuildingInfo.find(d => d.id === dossierId);
    const dossierBuilding = dossier && dossier.buildingInfoList.find(b => b.buildingId === buildingId) || {};
    return [200, dossierBuilding];
});

mock.onPost(URLS.UPLOAD_DOSSIER_FILES).reply((config) => {
    const allDossiers = Object.assign([], dossiers);
    const data = JSON.parse(config);
    return [200, data];
});

mock.onPost(URLS.UPDATE_FILES_PROPERTIES).reply((config) => {
    return [200, { status: 'success' }];
});

mock.onPost(URLS.UPDATE_DOSSIER_INFORMATION).reply((config) => {
    const { key, value, id } = JSON.parse(config.data);
    const index = dossierGeneralInfo.findIndex(d => d.id === id);
    if (index >= 0) {
        dossierGeneralInfo[index] = {
            ...dossierGeneralInfo[index],
            [key]: value
        }
        return [200, dossierGeneralInfo[index]];
    }
    return [200, {}];
});


mock.onPost(URLS.ADD_DOSSIER).reply((config) => {
    const allDossiers = Object.assign({}, dossiers);
    let data = JSON.parse(config.data);
    const isDraft = data.isDraft;
    const key = !isDraft ? 'openOrClosedDossiers' : 'draftDossiers';
    if (data.id) {
        const index = allDossiers[key].findIndex(p => p.id === data.id);
        allDossiers[key][index] = {
            ...allDossiers[key][index],
            ...data,
        }
    } else {
        data = {
            ...data,
            isArchived: false,
            id: Math.random().toString(16).slice(2) + (new Date()).getTime() + Math.random().toString(16).slice(2),
        }
        allDossiers[key].push(data);
    }
    return [200, data];
});
