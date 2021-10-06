import Axios from "axios";
import MockAdapter from 'axios-mock-adapter';

export const mock = new MockAdapter(Axios, { delayResponse: 2000 });