#!/bin/bash
set -e
pg_restore -U postgres -C -d wellsky  /wellsky.backup
