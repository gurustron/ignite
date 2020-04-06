/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package org.apache.ignite.internal.processors.platform.client.cache;

import org.apache.ignite.internal.binary.BinaryRawWriterEx;
import org.apache.ignite.internal.processors.platform.client.ClientConnectionContext;
import org.apache.ignite.internal.processors.platform.client.ClientResponse;
import org.apache.ignite.internal.processors.query.GridQueryFieldMetadata;

import java.util.List;

/**
 * Query cursor next page response.
 */
class ClientCacheSqlFieldsQueryMetaResponse extends ClientResponse {
    /** Cursor. */
    private final ClientCacheFieldsQueryCursor cursor;

    /**
     * Ctor.
     *
     * @param requestId Request id.
     * @param cursor Cursor.
     */
    ClientCacheSqlFieldsQueryMetaResponse(long requestId, ClientCacheFieldsQueryCursor cursor) {
        super(requestId);

        assert cursor != null;

        this.cursor = cursor;
    }

    /** {@inheritDoc} */
    @Override public void encode(ClientConnectionContext ctx, BinaryRawWriterEx writer) {
        super.encode(ctx, writer);

        List<GridQueryFieldMetadata> metadatas = cursor.fieldsMeta();

        writer.writeInt(metadatas.size());

        for (GridQueryFieldMetadata metadata : metadatas) {
            writer.writeString(metadata.fieldName());
            writer.writeString(metadata.fieldTypeName());
        }
    }
}
