import * as React from 'react';
import { IBrowseContext } from './BrowseProvider';
import { IBrowsedNode } from './service/IBrowsedNode';

export const BrowseContext = React.createContext<IBrowseContext>({
    nodes: new Map<string, IBrowsedNode>,
    visibleNodes: [],
    setVisibleNodes: () => { },
    browseChildren: (): Promise<void> => Promise.resolve(),
    readValues: (): Promise<void> => Promise.resolve(),
    responseCount: 0,
    processResults: () => { return []; }
});