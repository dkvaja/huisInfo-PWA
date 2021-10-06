import React from "react";
import { Router, Route, Redirect, matchPath, Switch } from "react-router-dom";
import { connect } from "react-redux";
import { createMuiTheme, withStyles } from '@material-ui/core/styles';
import { ThemeProvider } from '@material-ui/styles';
import { history, getCommonArray, authHeader } from "./_helpers";
import { alertActions, userActions, commonActions } from "./_actions";
import { Layout, LayoutInternal } from "./_layout";
import { HomePage, BuildingOverviewPage } from "./HomePage";
import {
  Dashboard,
  StandardOptionsPage,
  RequestIndividualOptionPage,
  MessagesPage,
  ThankYouPage,
  ThankYouOrderPage,
  GeneralProjectInfoPage,
  FaqPage,
  MyHomePage,
  DocumentsPage,
  InvolvedPartiesPage,
  StandardOptionsConfigPage,
  MyFinalOptionsPage,
  ThankYouOrderToBeSignedPage,
  MyRequestedOptionsPage,
  RepairRequestsHomePage,
  RepairRequestDetailsPage,
  AddNewRepairRequestPage,
  WorkOrderStatusUpdatePage,
  ProjectDashboard,
  ObjectsPage,

  ObjectLayout,
  ProjectLayout,
  DossiersPage,
  ConstructionQualityPage,
  RepairRequestWorkOrdersPage,
  WorkOrderDetailsPage,
  ResolverGridPage,
  ResolverWorkOrderDetailsPage, DossierDeadline, DossierDeadlineDetails,

} from "./Pages";
import { LoginPage, ForgotPage, ResetPage } from "./LoginPage";
import { userAccountTypeConstants, apps, appsInfo } from "./_constants";
import AutoReload from "./components/AutoReload";
import { RequestedPage } from "./Pages/HousingWishes/RequestedPage";
import { HomeInternal } from "./Pages/Internal/HomeInternal";
import axios from "axios";
import DossierRedirect from "./Pages/Dossiers/DossierRedirect";

const theme = createMuiTheme({
  overrides: {
    //MuiAppBar: {
    //    root: {
    //        background: '#fff'
    //    }
    //},
    MuiButton: {
      root: {
        textTransform: 'inherit',
        //"&:hover": {
        //    color: 'inherit'
        //}
      }
    },
    MUIDataTable: {
      paper: {
        ['@media print']: {
          height: 'auto !important',
          overflow: 'auto !important'
        }
      },
    },
  }
});

const GlobalCss = withStyles({
  // @global is handled by jss-plugin-global.
  '@global': {
    '*': {
      scrollbarColor: 'rgba(0,0,0,.2) hsla(0,0%,100%,.1)',
      scrollbarWidth: 'thin'
    },
    '::-webkit-scrollbar': {
      width: theme.spacing(0.5),
      height: theme.spacing(0.5),
    },
    '::-webkit-scrollbar-track': {
      background: 'hsla(0,0%,100%,.1)'
    },
    '::-webkit-scrollbar-thumb': {
      background: 'rgba(0,0,0,.2)',
      borderRadius: theme.spacing(1),
      '&:hover': {
        background: theme.palette.action.active,
      }
    }
  },
})(() => null);

class App extends React.Component {
  state = { pageTitle: '' };

  constructor(props) {
    super(props);

    const { dispatch } = this.props;
    var matchResult = matchPath(history.location.pathname, '/viewasbuyer/:loginId');
    if (matchResult) {
      dispatch(userActions.getViewAsUser(matchResult.params.loginId));
      history.push('/')
    }
    else {
      dispatch(userActions.getLoggedInUser());
    }
    history.listen((location, action) => {
      // clear alert on location change
      dispatch(alertActions.clear());
    });
  }

  componentDidMount() {
    this.updatePageTitle()
  }

  componentDidUpdate(prevProps, prevState) {
    if (
      (!prevProps.user && this.props.user)
      ||
      (prevProps.user && !this.props.user)
      ||
      (prevProps.user && this.props.user && prevProps.user.type !== this.props.user.type)
    ) {
      this.updatePageTitle();
      axios.defaults.headers = authHeader();
    }
    if (this.props.user && this.props.user.viewAsFailed === true) {
      alert(this.props.t('ViewAsFailed'));
      window.close();
    }
  }

  updatePageTitle() {
    const { webApiUrl } = window.appConfig;
    if (this.state.pageTitle && this.state.pageTitle.trim() !== '') {
      document.title = this.state.pageTitle;
    } else {
      var title = localStorage.getItem('pageTitle');
      document.title = title ? title : '';
    }

    const url = webApiUrl + 'api/config/GetPageTitle';
    const requestOptions = {
      method: 'GET',
      headers: authHeader()
    };

    fetch(url, requestOptions)
      .then(Response => Response.text())
      .then(findResponse => {
        document.title = findResponse;
        localStorage.setItem('pageTitle', findResponse);
        this.setState({
          pageTitle: findResponse
        });
      });
  }

  CheckAccessToSelectedAppsAndSwitch(...apps) {
    const userApps = getCommonArray(this.props.user.availableApps, apps);
    if (userApps.length > 0) {
      if (!apps.includes(this.props.app)) {
        this.props.dispatch(commonActions.selectApp(userApps[0]));
      }
      return true;
    }
    else {
      return false;
    }
  }

  render() {
    const { user, loggingIn, app } = this.props;
    return (
      <React.Suspense fallback={null}>
        <ThemeProvider theme={theme}>
          <Router history={history}>
            <React.Fragment>
              <GlobalCss />
              <AutoReload url="/index.html" tryDelay={10 * 60 * 1000} />
              {
                user && !matchPath(history.location.pathname, '/werkbon/:workOrderId') ?
                  <React.Fragment>
                    <Route
                      exact
                      path="/"
                      component={({ ...props }) => <Dashboard {...props} />}
                    />
                    {
                      history.location.pathname !== '/' && (
                        user.type === userAccountTypeConstants.buyer || (app === apps.aftercare) ?
                          <Layout user={user}>
                            {
                              <React.Fragment>
                                <Route
                                  exact
                                  path="/nazorg"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.aftercare) &&
                                    <RepairRequestsHomePage {...props} />)}
                                />
                                <Route
                                  exact
                                  path="/nazorg/details/:repairRequestId"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.aftercare) &&
                                    <RepairRequestDetailsPage {...props} />)}
                                />
                                <Route
                                  exact
                                  path="/nazorg/nieuwe"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.aftercare) &&
                                    <AddNewRepairRequestPage {...props} />)}
                                />
                              </React.Fragment>
                            }
                            {
                              <React.Fragment>
                                <Route
                                  exact
                                  path="/algemene-projectinformatie"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo, apps.aftercare) &&
                                    <GeneralProjectInfoPage {...props} />)}
                                />
                                {
                                  user.type === userAccountTypeConstants.buyer &&
                                  <Route
                                    exact
                                    path="/mijn-woning"
                                    render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo, apps.aftercare) &&
                                      <MyHomePage {...props} />)}
                                  />
                                }
                                <Route
                                  exact
                                  path="/betrokken-partijen"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo, apps.aftercare) &&
                                    <InvolvedPartiesPage {...props} />)}
                                />
                                <Route
                                  exact
                                  path="/veel-gestelde-vragen"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo, apps.aftercare) &&
                                    <FaqPage {...props} />)}
                                />
                                <Route
                                  exact
                                  path="/documenten"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo, apps.aftercare) &&
                                    <DocumentsPage {...props} />)}
                                />
                                <Route
                                  exact
                                  path={["/:viewType(dossier)", "/:viewType(dossier)/:selectedDataId"]}
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                    <DossiersPage {...props} />)}
                                />
                              </React.Fragment>
                            }
                            {
                              <React.Fragment>
                                <Route
                                  exact
                                  path="/home"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                    <HomePage {...props} />)}
                                />
                                <Route
                                  exact
                                  path="/berichten"
                                  render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                    <MessagesPage {...props} />)}
                                />
                                {
                                  user.type === userAccountTypeConstants.buyer &&
                                  <React.Fragment>
                                    <Route
                                      exact
                                      path="/keuzelijst"
                                      render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                        <StandardOptionsPage {...props} />)}
                                    />
                                    <Route
                                      exact
                                      path="/request-complete"
                                      render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                        <ThankYouPage {...props} />)}
                                    />
                                    {
                                      //    <Route
                                      //    exact
                                      //    path="/mijn-aangevraagde-opties"
                                      //    render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) && <MyRequestedOptionsPage {...props} />)}
                                      ///>
                                    }
                                    <Route
                                      exact
                                      path="/aangevraagd"
                                      render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                        <RequestedPage {...props} />)}
                                    />
                                    <Route
                                      exact
                                      path="/order-complete"
                                      render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                        <ThankYouOrderPage {...props} />)}
                                    />
                                    <Route
                                      exact
                                      path="/order-unsigned"
                                      render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                        <ThankYouOrderToBeSignedPage {...props} />)}
                                    />
                                    <Route
                                      exact
                                      path="/mijndefinitieveopties"
                                      render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                        <MyFinalOptionsPage {...props} />)}
                                    />
                                  </React.Fragment>
                                }
                                {
                                  user.type !== userAccountTypeConstants.buyer &&
                                  <React.Fragment>
                                    {
                                      //<Route
                                      //    exact
                                      //    path="/gebouw-overzicht"
                                      //    component={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) && <BuildingOverviewPage {...props} />)}
                                      ///>
                                    }
                                    {
                                      //<Route
                                      //    exact
                                      //    path="/standaard-opties"
                                      //    component={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) && <StandardOptionsConfigPage {...props} />)}
                                      ///>
                                    }
                                  </React.Fragment>
                                }
                              </React.Fragment>
                            }
                          </Layout>
                          :
                          <LayoutInternal user={user} history={history}>
                            <Route
                              exact
                              path="/home"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <HomeInternal />)}
                            />
                            <Route
                              exact
                              path="/kwaliteitsborging"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.constructionQuality) &&
                                <HomeInternal />)}
                            />
                            <Route
                              exact
                              path="/werkbonnen"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.resolverModule) &&
                                <HomeInternal />)}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/kwaliteitsborging"
                              component={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.constructionQuality) &&
                                <ProjectLayout {...props}>
                                  <ConstructionQualityPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/kwaliteitsborging/:repairRequestId"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.constructionQuality) &&
                                <ProjectLayout {...props}>
                                  <RepairRequestWorkOrdersPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/werkbon/:resolverId"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.constructionQuality) &&
                                <ProjectLayout {...props}>
                                  <WorkOrderDetailsPage {...props} />
                                </ProjectLayout>
                              )}
                            />

                            <Route
                              exact
                              path="/werk/:projectNo/werkbonnen"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.resolverModule) &&
                                <ProjectLayout {...props}>
                                  <ResolverGridPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/werkbon/:resolverId"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.resolverModule) &&
                                <ProjectLayout {...props}>
                                  <ResolverWorkOrderDetailsPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <ProjectDashboard />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/objecten"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <ObjectsPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/berichten"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <MessagesPage isFullWidth={true} {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/werk/:projectNo/standaard-opties"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <StandardOptionsConfigPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path={["/werk/:projectNo/:viewType(dossier|building)", "/werk/:projectNo/:viewType(dossier|building)/:selectedDataId"]}
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <DossiersPage {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              path={"/dossier/:dossierId"}
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                // <ProjectLayout {...props}>
                                <DossierRedirect {...props} />
                                // </ProjectLayout>
                              )}
                            />
                            {/* <Route
                                                          path={"/werk/:projectNo/building"}
                                                          render={({...props}) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                                            <ProjectLayout {...props}>
                                                                <DossiersPage {...props} />
                                                            </ProjectLayout>
                                                          )}
                                                        /> */}

                            <Route
                              exact
                              path={"/werk/:projectNo/deadline-dossier"}
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <DossierDeadline {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path={"/werk/:projectNo/deadline-dossier/details"}
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ProjectLayout {...props}>
                                  <DossierDeadlineDetails {...props} />
                                </ProjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/object/:buildingNoInternal"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ObjectLayout {...props}>
                                  <BuildingOverviewPage {...props} />
                                </ObjectLayout>
                              )}
                            />
                            <Route
                              exact
                              path="/object/:buildingNoInternal/berichten"
                              render={({ ...props }) => (this.CheckAccessToSelectedAppsAndSwitch(apps.huisinfo) &&
                                <ObjectLayout {...props}>
                                  <MessagesPage isFullWidth={true} selectedBuildingOnly={true} {...props} />
                                </ObjectLayout>
                              )}
                            />
                          </LayoutInternal>
                      )
                    }
                  </React.Fragment>
                  :
                  (
                    loggingIn !== true
                    && history.location.pathname !== '/login'
                    && history.location.pathname !== '/forgot'
                    && history.location.pathname !== '/reset'
                    && !matchPath(history.location.pathname, '/werkbon/:workOrderId')
                    &&
                    <Redirect to={{ pathname: '/login', state: { from: history.location } }} />
                  )
              }
              <Route path="/login" component={LoginPage} />
              <Route path="/forgot" component={ForgotPage} />
              <Route path="/reset" component={ResetPage} />
              <Route path="/werkbon/:resolverId" component={WorkOrderStatusUpdatePage} />
            </React.Fragment>
          </Router>
        </ThemeProvider>
      </React.Suspense>
    );
  }
}

function mapStateToProps(state) {
  const { alert, authentication, app } = state;
  const { user, loggingIn } = authentication;
  return {
    alert,
    user,
    loggingIn,
    app
  };
}

export default connect(mapStateToProps)(App);
