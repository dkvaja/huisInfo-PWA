import React, { Component } from "react";
import packageJson from '../../package.json';
import { authHeader } from "../_helpers";
import {unregister} from "../registerServiceWorker";

const { webApiUrl } = window.appConfig;

class AutoReload extends Component {
    constructor(props) {
        super(props);
        this.state = {
            codeHasChanged: false,
        };
        this.fetchSource = this.fetchSource.bind(this);
    }

    componentDidMount() {
        const { tryDelay } = this.props;
        const un = localStorage.getItem('sw-un');
        if (!un) this.reloadApp();
        this.fetchSource();
        this.interval = setInterval(this.fetchSource, tryDelay);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    fetchSource() {
        const url = webApiUrl + 'api/config/CheckApiStatus';
        const requestOptions = {
            method: 'GET',
            headers: authHeader()
        };

        this.setState({
            categories: [],
            options: [],
            searchTerm: ''
        });

        fetch(url, requestOptions)
            .then(Response => Response.json())
            .then(findResponse => {
                if (findResponse.version.toUpperCase() !== packageJson.version.toUpperCase())
                    this.setState({ codeHasChanged: true });
            })
            .catch(() => {
                //do nothing
            });
    }

    async reloadApp(e) {
        if(e) e.preventDefault();
        localStorage.setItem('sw-un', "true");
        await unregister();
        if (caches) {
            const keys = await caches.keys();
            for (let name of keys) {
                await caches.delete(name);
            }
        }
        window.location.reload(true);
    }
    render() {
        const style = {
            position: "absolute",
            top: 10,
            right: 10,
            padding: "1em",
            zIndex: 1150,
            backgroundColor: "bisque",
            borderRadius: 5,
            textAlign: "center",
        };
        //Replace this with Material-UI Snackbar
        return (
            <React.Fragment>
                <input type="hidden" name="version" value={packageJson.version} />
                {
                    // Added to remove from dev env...
                    !!this.state.codeHasChanged && process.env.NODE_ENV === 'production' ?
                        <div style={style}>
                            <div>Huisinfo heeft een nieuwe versie.</div>
                            <div>
                                <a href="#" onClick={this.reloadApp}>Klik hier om te herladen</a>
                            </div>
                        </div>
                        :
                        null
                }
            </React.Fragment>
        );
    }
}

AutoReload.defaultProps = {
    url: "/",
    tryDelay: 5 * 60 * 1000, // 5 minutes
    forceDelay: 1 * 60 * 60 * 1000, // 1 hour
};

export default AutoReload;
