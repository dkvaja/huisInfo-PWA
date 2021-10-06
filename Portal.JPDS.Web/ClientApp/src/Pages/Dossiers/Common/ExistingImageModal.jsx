import React, { useState } from 'react';
import {
	Button,
	Checkbox,
	Container,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	IconButton,
	makeStyles,
	Typography,
	Grid,
	Tooltip
} from '@material-ui/core';
import { Close, Visibility } from '@material-ui/icons';
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { URLS } from '../../../apis/urls';

const ExistingImageModal = ({ open, handleClose, isSingle, handlePreviewOfFiles, onselect = () => 0, ...props }) => {
	const classes = useStyles();
	const { t } = useTranslation();
	const [selectedImages, setSelectedImages] = useState([]);
	const { images } = useSelector(state => state.dossier);


	const handleSelect = (isAll, file) => {
		let images = Object.assign([], selectedImages);
		if (!isAll && !isSingle) {
			const isExistIndex = images.findIndex(image => image.fileId === file.fileId);
			isExistIndex >= 0 ? images.splice(isExistIndex, 1) : images.push(file)
		} else if (isSingle) {
			images = images.find(f => f.fileId === file.fileId) ? [] : [file];
		} else if (!isSingle && isAll)
			images = images.length ? [] : [...file];
		setSelectedImages(images)
	}

	return (
		<Dialog maxWidth={'md'} PaperProps={{ style: { width: 'auto' } }} onClose={handleClose} aria-labelledby="images-dialog-title" open={open} >
			<DialogTitle id="images-dialog-title" className={classes.titleContainer}>
				<Container className={classes.title}>
					{!isSingle && <>
						<Checkbox
							color="primary"
							indeterminate={(images.length && selectedImages.length !== images.length) && selectedImages.length}
							checked={images.length && selectedImages.length === images.length}
							onChange={() => handleSelect(true, images)}
						/>
						<Typography variant={'p'} className={classes.totalImagesData}>{`(${selectedImages.length}/${images.length})`}</Typography>
					</>
					}
					<Typography variant={'h5'} className={classes.totalImagesHeading}>{t('files.modal.heading.title')}</Typography>
					<IconButton onClick={handleClose} aria-label="close">
						<Close />
					</IconButton>
				</Container>
			</DialogTitle>
			<DialogContent className={classes.container}>
				<Grid className={classes.dialogWidthController}>
					<Grid container spacing={1}>
						{
							images && images.map((image, index) => {
								const isSelected = selectedImages.some(f => f.fileId === image.fileId);
								return (
									<Grid item key={index} className={classes.fileContainer} justify='center'>
										<Tooltip title={image.name}>
											<Grid item className={classes.thumnailContainer}>
												<div className={classes.overlay}>
													<IconButton onClick={() => handlePreviewOfFiles([image])}
														style={{ outline: 'none' }}
														aria-label="image-preview">
														<Visibility className={classes.imagePreviewIcon} />
													</IconButton>
													{!isSelected && <Checkbox checked={isSelected}
														onChange={() => handleSelect(false, image)} color="primary"
														className={classes.checkBox}
														style={{ color: 'white' }} />}
												</div>
												{isSelected && <Checkbox checked={isSelected}
													onChange={() => handleSelect(false, image)} color="primary"
													className={classes.checkBox}
													style={{ zIndex: 5 }} />}
												<div className={classes.thumbnail}
													style={{ backgroundImage: `url(${URLS.GET_ATTACHMENT_THUMBNAIL}${image.fileId})` }}>{ }</div>
												<Typography variant="caption" className={classes.caption} noWrap>{image.name}</Typography>
											</Grid>
										</Tooltip>
									</Grid>
								)
							})
						}
					</Grid>
				</Grid>
			</DialogContent>
			{!!selectedImages.length && <DialogActions>
				<Button color="primary" onClick={() => {
					onselect(selectedImages);
					handleClose();
				}}>
					{t('files.button.select.title')}
				</Button>
			</DialogActions>}
		</Dialog>
	)
}

const useStyles = makeStyles((theme) => ({
	avatar: {
		width: theme.spacing(9),
		height: theme.spacing(9),
	},
	container: {
		padding: theme.spacing(1, 1.5),
	},
	dialogWidthController: {
		maxWidth: 888,
		width: 'max-content',
		[theme.breakpoints.down('md')]: {
			maxWidth: 760,
		},
		[theme.breakpoints.down('sm')]: {
			maxWidth: 504,
		},
		[theme.breakpoints.down('xs')]: {
			maxWidth: 248,
			margin: 'auto',
		},
	},
	imageName: {
		marginLeft: 20
	},
	totalImagesData: {
		lineHeight: 1.334,
		[theme.breakpoints.down('xs')]: {
			fontSize: '1rem',
		},
		[theme.breakpoints.down(360)]: {
			fontSize: '0.9rem',
		}
	},
	totalImagesHeading: {
		flexGrow: 1,
		whiteSpace: 'nowrap',
		[theme.breakpoints.down('xs')]: {
			fontSize: '1rem',
		},
		[theme.breakpoints.down(360)]: {
			fontSize: '0.9rem',
		},

	},
	titleContainer: {
		padding: theme.spacing(1, 2),
		[theme.breakpoints.down('xs')]: {
			padding: theme.spacing(1, 0),
		}
	},
	title: {
		display: 'flex',
		justifyContent: 'center',
		alignItems: 'center',
		padding: 0
	},
	caption: {
		width: '100%',
		textAlign: 'center'
	},
	thumbnail: {
		backgroundPosition: 'center',
		backgroundSize: 'contain',
		backgroundRepeat: 'no-repeat',
		height: 80,
		padding: '35% 0',
		width: '100%',
	},
	checkBox: {
		position: 'absolute',
		right: 0,
		top: 0,
		padding: 0
	},
	imagePreviewIcon: {
		fill: theme.palette.common.white,
		width: '1.5em',
		height: '1.5em'
	},
	overlay: {
		position: 'absolute',
		background: 'rgba(0, 0, 0, 0.5)', /* Black see-through */
		width: '100%',
		transition: '.5s ease',
		opacity: 0,
		display: 'flex',
		flexDirection: 'column',
		justifyContent: 'center',
		alignItems: 'center',
		zIndex: 2,
		height: '100%',
	},
	fileContainer: {
		display: 'flex',
	},
	thumnailContainer: {
		minHeight: 100,
		display: 'flex',
		height: 120,
		width: 120,
		flexDirection: 'column',
		justifyContent: 'center',
		alignItems: 'center',
		position: 'relative',
		cursor: 'pointer',
		'&:hover': {
			"& $overlay": {
				opacity: 1
			},
		},
	}
}));

export default ExistingImageModal;
