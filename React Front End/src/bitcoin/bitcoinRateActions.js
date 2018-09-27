// in src/bitcoinRateReceived.js
export const BITCOIN_RATE_RECEIVED = 'BITCOIN_RATE_RECEIVED';
export const BITCOIN_RATE_SET = 'BITCOIN_RATE_SET';
export const bitcoinRateReceived = (rate) => ({
    type: BITCOIN_RATE_RECEIVED,
    payload: { rate },
});

export const bitcoinRateSet = (rate) => ({
    type: BITCOIN_RATE_SET,
    payload: { rate },
});